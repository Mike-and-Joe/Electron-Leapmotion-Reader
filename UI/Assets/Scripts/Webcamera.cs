using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using OpenCvSharp;

public class Webcamera : MonoBehaviour, RecorderInterface
{

    private bool camAvailable;
    private WebCamTexture cameraTexture;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;
    public bool frontFacing;

    private int cameraIndex;

    // Use these in RecordingMethod
    private bool isRecording;

    // Use this for initialization
    void Start()
    {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
            return;

        for (int i = 0; i < devices.Length; i++)
        {
            var curr = devices[i];
            print("Devices " + i + " , name: "+ curr.name + ", font? :" + curr.isFrontFacing);

            if (curr.isFrontFacing == frontFacing && curr.name != "Leap Dev Kit")
            {
                cameraTexture = new WebCamTexture(curr.name, Screen.width, Screen.height);
                cameraIndex = i;
                break;
            }
        }

        if (cameraTexture == null)
            return;

        cameraTexture.Play(); // Start the camera
        background.texture = cameraTexture; // Set the texture

        camAvailable = true; // Set the camAvailable for future purposes.
        isRecording = false;
    }

    void OnDisable ()
    {
        cameraTexture.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (!camAvailable)
            return;

        float ratio = (float)cameraTexture.width / (float)cameraTexture.height;
        fit.aspectRatio = ratio; // Set the aspect ratio

        float scaleY = cameraTexture.videoVerticallyMirrored ? -1f : 1f; // Find if the camera is mirrored or not
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f); // Swap the mirrored camera

        int orient = -cameraTexture.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

    public void StartRecording(int projectNo, int gestureNo, int subjectNo, int indexOfSubjectGesture)
    {
        isRecording = true;

        VideoCapture capture0 = new VideoCapture(cameraIndex);
        Mat frame = new Mat();
        while (isRecording)
        {
            capture0.Read(frame);
            Cv2.ImShow("frame", frame);
        }
        capture0.Release();
        Cv2.DestroyAllWindows();

        //// Opens a camera device
        //using (VideoCapture capture = new VideoCapture(cameraIndex))
        //// Read movie frames and write them to VideoWriter 
        ////new Size(capture.FrameWidth, capture.FrameHeight)
        //using (VideoWriter writer = new VideoWriter("out.avi", VideoWriter.FourCC('M', 'J', 'P', 'G'), capture.Fps, new Size(capture.FrameWidth, capture.FrameHeight)))
        //using (Mat frame = new Mat())
        ////using (Mat gray = new Mat())
        ////using (Mat canny = new Mat())
        ////using (Mat dst = new Mat())
        //{
        //    print("Converting each movie frames...");
        //    print(capture.FrameHeight + ", " + capture.FrameWidth + " : " + capture.Fps);

        //    Size dsize = new Size(capture.FrameWidth, capture.FrameHeight);

        //    while (isRecording)
        //    {
        //        // Read image
        //        capture.Read(frame);
        //        if (frame.Empty())
        //            break;

        //        //Cv2.ImShow("face camera", frame);

        //        print(capture.PosFrames + " / " + capture.FrameCount);

        //        //// grayscale -> canny -> resize
        //        //Cv2.CvtColor(frame, gray, VideoWriter.ColorConversion.BgrToGray);
        //        //Cv2.Canny(gray, canny, 100, 180);
        //        //Cv2.Resize(canny, dst, dsize, 0, 0, VideoWriter.Interpolation.Linear);
        //        // Write mat to VideoWriter
        //        writer.Write(frame);
        //    }

        //    capture.Release();
        //    Cv2.DestroyAllWindows();
        //}

        print("out");
    }

    public void StopRecording()
    {
        isRecording = false;
    }
}