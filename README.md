# HoloLens Windows MachineLearning

The way to train your model in the Cloud and run it locally on the HoloLens!

The minimal sample code. The objectives are:
- Capture frames from the HoloLens camera
- Load the MachineLearning model
- Execute the model and get the predictions from each captured frame

> **Note:** Windows version 1809 i.e. build > 17738 is needed to be able to run the Windows.AI.MachineLearning APIs


## [HoloLensV2WinML](./HoloLensV2WinML) ➡ HOLOLENS V2

The app takes the live stream of the HoloLens camera and evalute locally the ONNX model to recognize up/down thumbs.
The app targets HoloLens V2 in 32bits ARM.

### Versions used
- Windows version 1809 for the PC
- HoloLens V2
- Visual Studio 2019
- Windows 10 SDK 10.0.18362.0
- Unity 2017.8.3.8f1


## [HoloLensWinMLmin](./HoloLensWinMLmin) ➡ HOLOLENS V1

Same app for HoloLens V1.

### Versions used
- Windows version 1809 for the PC and HoloLens
- Visual Studio 2017 version 15.9.0
- Windows SDK version 10.0.17763.0
- Unity 2017.4.13f1
