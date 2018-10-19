using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



public class CameraHelper
{


    //public async void CreateMediaCapture()
    //{
    //    MediaCapture = new MediaCapture();
    //    MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings();
    //    settings.StreamingCaptureMode = StreamingCaptureMode.Video;
    //    await MediaCapture.InitializeAsync(settings);

    //    CreateFrameReader();
    //}



    //private async void CreateFrameReader()
    //{
    //    var frameSourceGroups = await MediaFrameSourceGroup.FindAllAsync();

    //    MediaFrameSourceGroup selectedGroup = null;
    //    MediaFrameSourceInfo colorSourceInfo = null;

    //    foreach (var sourceGroup in frameSourceGroups)
    //    {
    //        foreach (var sourceInfo in sourceGroup.SourceInfos)
    //        {
    //            if (sourceInfo.MediaStreamType == MediaStreamType.VideoPreview
    //                && sourceInfo.SourceKind == MediaFrameSourceKind.Color)
    //            {
    //                colorSourceInfo = sourceInfo;
    //                break;
    //            }
    //        }
    //        if (colorSourceInfo != null)
    //        {
    //            selectedGroup = sourceGroup;
    //            break;
    //        }
    //    }

    //    var colorFrameSource = MediaCapture.FrameSources[colorSourceInfo.Id];
    //    var preferredFormat = colorFrameSource.SupportedFormats.Where(format =>
    //    {
    //        return format.Subtype == MediaEncodingSubtypes.Argb32;

    //    }).FirstOrDefault();

    //    var mediaFrameReader = await MediaCapture.CreateFrameReaderAsync(colorFrameSource);
    //    await mediaFrameReader.StartAsync();
    //    StartPullFrames(mediaFrameReader);
    //}


    //private void StartPullFrames(MediaFrameReader sender)
    //{
    //    Task.Run(async () =>
    //    {
    //        for (; ; )
    //        {
    //            nbFrames++;
    //            System.Diagnostics.Debug.Write(".");
    //            await Task.Delay(predictEvery);
    //            using (var frameReference = sender.TryAcquireLatestFrame())
    //            using (var videoFrame = frameReference?.VideoMediaFrame?.GetVideoFrame())
    //            {







    //                if (videoFrame == null)
    //                {
    //                    continue; //ignoring frame
    //                }

    //                if (videoFrame.Direct3DSurface == null)
    //                {
    //                    videoFrame.Dispose();
    //                    continue; //ignoring frame
    //                }

    //                try
    //                {
    //                    System.Diagnostics.Debug.WriteLine("Evalutation");
    //                    await EvaluateVideoFrameAsync(videoFrame).ConfigureAwait(false); ;


    //                    //if (prediction.Loss[classWithHighestProb] > 0.5)
    //                    //{
    //                    //    DisplayText("I see a " + classWithHighestProb);
    //                    //}
    //                    //else
    //                    //{
    //                    //    DisplayText("I see nothing");
    //                    //}
    //                }
    //                catch
    //                {
    //                    //Log errors
    //                }
    //                finally
    //                {
    //                    i++;
    //                }
    //            }

    //        }

    //    });
    //}
}

