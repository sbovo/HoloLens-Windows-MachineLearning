#if UNITY_WSA && !UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using Windows.Media;
using Windows.Storage;

public class ONNXModelHelper
{
    private ONNXModel Model = null;
    private string ModelFilename = "ONNXModel.onnx";
    private Stopwatch TimeRecorder = new Stopwatch();
    private IUnityScanScene UnityApp;

    public ONNXModelHelper()
    {
        UnityApp = null;
    }

    public ONNXModelHelper(IUnityScanScene unityApp)
    {
        UnityApp = unityApp;
    }

    public async Task LoadModelAsync()
    {
        ModifyText($"Loading {ModelFilename}... Patience");

        try
        {
            TimeRecorder = Stopwatch.StartNew();

            var modelFile = await StorageFile.GetFileFromApplicationUriAsync(
                new Uri($"ms-appx:///Data/StreamingAssets/{ModelFilename}"));
            Model = await ONNXModel.CreateOnnxModel(modelFile);

            TimeRecorder.Stop();
            System.Diagnostics.Debug.WriteLine($"Loaded {ModelFilename}: Elapsed time: {TimeRecorder.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"error: {ex.Message}");
            ModifyText($"error: {ex.Message}");
            Model = null;
        }
    }

    public async Task EvaluateVideoFrameAsync(VideoFrame frame)
    {
        if (frame != null)
        {
            try
            {
                TimeRecorder.Restart();
                ONNXModelInput inputData = new ONNXModelInput();
                inputData.Data = frame;
                var output = await Model.EvaluateAsync(inputData).ConfigureAwait(false);

                var product = output.ClassLabel.GetAsVectorView()[0];
                var loss = output.Loss[0][product];
                TimeRecorder.Stop();

                var lossStr = string.Join(product, " " + (loss * 100.0f).ToString("#0.00") + "%");
                string message = $"({DateTime.Now.Minute}:{DateTime.Now.Second})" +
                    $" Evaluation took {TimeRecorder.ElapsedMilliseconds}ms to execute\n" +
                    $"Predictions: {lossStr}";
                message = message.Replace("\\n", "\n");

                //String allPredictionString = "(" + i + ")" + Environment.NewLine +
                //    output.ClassLabel.GetAsVectorView()[0] + " - "
                //        + (output.Loss[0][output.ClassLabel.GetAsVectorView()[0]] * 100.0f).ToString("#0.00") + "%"
                //        + Environment.NewLine +
                //    output.ClassLabel.GetAsVectorView()[1] + " - "
                //        + (output.Loss[1][output.ClassLabel.GetAsVectorView()[1]] * 100.0f).ToString("#0.00") + "%"
                //        + Environment.NewLine;



                System.Diagnostics.Debug.WriteLine(message);
                //System.Diagnostics.Debug.WriteLine(allPredictionString);

                ModifyText(message);
            }
            catch (Exception ex)
            {
                var err_message = $"error: {ex.Message}";
                System.Diagnostics.Debug.WriteLine(err_message);
                ModifyText(err_message);
            }
        }
    }


    private void ModifyText(string text)
    {

        if (UnityApp != null)
        {
            UnityApp.ModifyOutputText(text);
        }
    }
}
#endif