using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System;
using System.Threading;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

using Leap;


public class SampleListener
{
    private bool isPredicting = false;
    private string data = "";
    private int count = 0;

    public void OnServiceConnect(object sender, ConnectionEventArgs args)
    {
        Debug.Log("Service Connected");
    }

    public void OnConnect(object sender, DeviceEventArgs args)
    {
        Debug.Log("Connected");
    }

    public void OnFrame(object sender, FrameEventArgs args)
    {
        if (isPredicting)
        {
            Frame frame = args.frame;
            string frameStr = "";

            if (data.Length != 1)
            {
                frameStr += ", ";
            }

            frameStr += "{\"frame_id\": "+ frame.Id +",";

            frameStr += "\"hands\": {";
            foreach (Hand hand in frame.Hands)
            {
                string handName = hand.IsLeft ? ",\"left\"" : "\"right\"";
                frameStr += handName + ": {";

                frameStr += " \"hand_palm_position\": [ " + hand.PalmPosition.x +", " + hand.PalmPosition.y + ", " + hand.PalmPosition.z + "],";


                Vector normal = hand.PalmNormal;
                Vector direction = hand.Direction;
                frameStr += "\"pitch\":" + direction.Pitch * 180.0f / (float)Math.PI + ",";
                frameStr += "\"roll\":" + normal.Roll * 180.0f / (float)Math.PI + ",";
                frameStr += "\"yaw\":" + direction.Yaw * 180.0f / (float)Math.PI + ",";


                frameStr += "\"fingers\": {";

                bool isStart = true;
                string[] fingersName = new string[] { "thumb", "index", "middle", "ring", "pinky" };
                foreach (Finger finger in hand.Fingers)
                {
                    if (!isStart)
                    {
                        frameStr += ",";
                    }
                    isStart = false;

                    frameStr += "\"" + fingersName[(int)finger.Type] + "\": {";

                    //Vector tip = finger.TipPosition;
                    Bone distalBone = finger.Bone(Bone.BoneType.TYPE_DISTAL);
                    Bone proximalBone = finger.Bone(Bone.BoneType.TYPE_PROXIMAL);
                    Vector tipFinger = distalBone.NextJoint;
                    Vector proximalBoneVector = proximalBone.Direction;

                    frameStr += "\"bones\": {";
                    frameStr +=     "\"distal\": {";
                    frameStr +=         "\"next_joint\":  [ " + tipFinger.x +", " + tipFinger.y + ", " + tipFinger.z + "]";
                    frameStr +=     "},";
                    frameStr +=     "\"proximal\": {";
                    frameStr +=         "\"direction\":  [ " + tipFinger.x + ", " + tipFinger.y + ", " + tipFinger.z + "]";
                    frameStr +=     "}";
                    frameStr += "}";


                    frameStr += "}";
                }
                frameStr += "}";

                frameStr += "}";
            }

            frameStr += "}}";
            data += frameStr;
        }
    }

    public void StartPredicting()
    {
        data = "[";
        count = 0;
        isPredicting = true;
    }

    public string StopPredicting()
    {
        isPredicting = false;
        data += " ]";
        return data;
        //return "{ \"leap_data\":\"OKKKKKK\" }";
    }
}


public class Prediction : MonoBehaviour
{
    public Button Button;
    public Text ResultText;
    public Text CountingText;
    private Text recordBtText;
    private float startTime;
    private bool isPredicting = false;
    SampleListener listener;
    Controller controller;

    private void Start()
    {
        recordBtText = Button.GetComponentInChildren(typeof(Text)) as Text;

        isPredicting = false;
        ResultText.text = "None";
        CountingText.text = "0 s";

        startLeap();

        StartCoroutine(postRequest("http://localhost:5000/test", "{ \"leap_data\":\"Connected with Server\" }"));

    }

    public void Update()
    {
        if (isPredicting)
        {
            CountingText.text = ((Time.time - startTime) % 60).ToString("00") + " s";
        }
    }

    public void startLeap()
    {
        controller = new Controller();
        listener = new SampleListener();
        controller.Connect += listener.OnServiceConnect;
        controller.Device += listener.OnConnect;
        controller.FrameReady += listener.OnFrame;

        //controller.RemoveListener(listener);
        //controller.Dispose();
    }

    public void TogglePredicting()
    {
        if (isPredicting)
        {
            recordBtText.text = "Record";
            StopPredicting();
        }
        else
        {
            recordBtText.text = "Stop";
            StartPredicting();
        }
    }

    private void StartPredicting()
    {
        isPredicting = true;
        listener.StartPredicting();
        ResultText.text = "Recording....";
        startTime = Time.time;
        //Task.WaitAll(tasks.ToArray());
    }

    private void StopPredicting()
    {
        isPredicting = false;
        String data = listener.StopPredicting();
        string body = "{ \"leap_data\": [" + data + "]}";
        StartCoroutine(postRequest("http://localhost:5000/predict", body));
    }

    IEnumerator postRequest(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            ResultText.text = uwr.downloadHandler.text;
        }
    }
}