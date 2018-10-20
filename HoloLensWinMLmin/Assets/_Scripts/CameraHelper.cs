using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Media.Capture;
using Windows.Media.Capture.Frames;
using Windows.Media.MediaProperties;

public class CameraHelper
{
    MediaCapture CameraCapture;
    MediaFrameReader CameraFrameReader;


    public CameraHelper()
    {
        InitializeCameraCapture();
        InitializeCameraFrameReader();
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

        var mediaFrameReader = await CameraCapture.CreateFrameReaderAsync(colorFrameSource);
        await mediaFrameReader.StartAsync();
    }


   
}

