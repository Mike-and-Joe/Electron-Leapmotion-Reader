using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Threading;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class Recording : MonoBehaviour {

    public Webcamera webcamera;
    public Webcamera leapcamera;
    public Webcamera leapcoordinate;
    public Button recordButton;
    public List<RecorderInterface> recorders;
    private Text recordBtText;
    private bool isRecording = false;

    private void Start()
    {
        recordBtText = recordButton.GetComponentInChildren(typeof(Text)) as Text;

        isRecording = false;

        print(webcamera);
        if (webcamera == null)
        {
            print("nulllll");
        }
        recorders = new List<RecorderInterface>();
        recorders.Add(webcamera);
        //recorders.Add(leapcamera);
        //recorders.Add(leapcoordinate);
    }

    public void ToggleRecording ()
    {
        if (isRecording)
        {
            recordBtText.text = "Record";
            StopRecording();
        } else
        {
            recordBtText.text = "Stop";
            StartRecording();
        }
    }

    private void StartRecording ()
    {
        print("Clicked!");
        isRecording = true;

        List<Task> tasks = new List<Task>();

        foreach (var recorder in recorders)
        {
            tasks.Add(Task.Factory.StartNew(() => recorder.StartRecording(0, 0, 0, 0)));
        }
        
        //Task.WaitAll(tasks.ToArray());
    }

    private void StopRecording ()
    {
        print("Stop!");
        isRecording = false;

        foreach (var recorder in recorders)
        {
            recorder.StopRecording();
        }
    }
}
