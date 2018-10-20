#if UNITY_WSA && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.Capture.Frames;
using Windows.Media.MediaProperties;

public class CameraHelper
{

    public TimeSpan PredictionFrequency = TimeSpan.FromMilliseconds(400);


    private MediaCapture CameraCapture;
    private MediaFrameReader CameraFrameReader;

    private Int64 FramesCaptured;


    public CameraHelper()
    {
        InitializeCameraCapture();
        InitializeCameraFrameReader();
        StartPullCameraFrames();
    }

    private async void InitializeCameraCapture()
    {
        CameraCapture = new MediaCapture();
        MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings();
        settings.StreamingCaptureMode = StreamingCaptureMode.Video;
        await CameraCapture.InitializeAsync(settings);
    }



    private async void InitializeCameraFrameReader()
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

    private void StartPullCameraFrames()
    {
        Task.Run(async () =>
        {
            while (true) // Forever = While the app runs
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
                        //await EvaluateVideoFrameAsync(videoFrame).ConfigureAwait(false); ;
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