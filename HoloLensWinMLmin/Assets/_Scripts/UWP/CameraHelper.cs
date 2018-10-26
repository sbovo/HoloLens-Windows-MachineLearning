#if UNITY_WSA && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Windows.Media.Capture;
using Windows.Media.Capture.Frames;
using Windows.Media.MediaProperties;

public class CameraHelper
{

    public TimeSpan PredictionFrequency = TimeSpan.FromMilliseconds(400);


    private MediaCapture CameraCapture;
    private MediaFrameReader CameraFrameReader;

    private Int64 FramesCaptured;

    ONNXModelHelper ModelHelper;


    public CameraHelper()
    { 
    }

    public async Task Inititalize(IUnityScanScene unityApp)
    {
        ModelHelper = new ONNXModelHelper(unityApp);
        await ModelHelper.LoadModelAsync();

        await InitializeCameraCapture();
        await InitializeCameraFrameReader();
    }

    private async Task InitializeCameraCapture()
    {
        CameraCapture = new MediaCapture();
        MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings();
        settings.StreamingCaptureMode = StreamingCaptureMode.Video;
        await CameraCapture.InitializeAsync(settings);
    }



    private async Task InitializeCameraFrameReader()
    {
        var frameSourceGroups = await MediaFrameSourceGroup.FindAllAsync(); 
        MediaFrameSourceGroup selectedGroup = null;
        MediaFrameSourceInfo colorSourceInfo = null;

        foreach (var sourceGroup in frameSourceGroups)
        {
            foreach (var sourceInfo in sourceGroup.SourceInfos)
            {
                if (sourceInfo.MediaStreamType == MediaStreamType.VideoPreview
                    && sourceInfo.SourceKind == MediaFrameSourceKind.Color)
                {
                    colorSourceInfo = sourceInfo;
                    break;
                }
            }
            if (colorSourceInfo != null)
            {
                selectedGroup = sourceGroup;
                break;
            }
        }

        var colorFrameSource = CameraCapture.FrameSources[colorSourceInfo.Id];
        var preferredFormat = colorFrameSource.SupportedFormats.Where(format =>
        {
            return format.Subtype == MediaEncodingSubtypes.Argb32;

        }).FirstOrDefault();

        CameraFrameReader = await CameraCapture.CreateFrameReaderAsync(colorFrameSource);
        await CameraFrameReader.StartAsync();
    }

    public void StartPullCameraFrames(IUnityScanScene unityApp)
    {
        Task.Run(async () =>
        {
            for (; ; ) // Forever = While the app runs
            {
                FramesCaptured++;
                System.Diagnostics.Debug.Write(".");
                await Task.Delay(PredictionFrequency);
                using (var frameReference = CameraFrameReader.TryAcquireLatestFrame())
                using (var videoFrame = frameReference?.VideoMediaFrame?.GetVideoFrame())
                {

                    if (videoFrame == null)
                    {
                        continue; //ignoring frame
                    }

                    if (videoFrame.Direct3DSurface == null)
                    {
                        videoFrame.Dispose();
                        continue; //ignoring frame
                    }

                    try
                    {
                        System.Diagnostics.Debug.WriteLine("Evalutation");
                        //unityApp.ModifyOutputText($"{System.DateTime.Now.Minute}:{System.DateTime.Now.Second}");
                        await ModelHelper.EvaluateVideoFrameAsync(videoFrame).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                    finally
                    {
                    }
                }

            }

        });
    }

}

#endif