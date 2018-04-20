using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class QuitProgram : MonoBehaviour {

    public void Quit()
    {
        Cv2.DestroyAllWindows();
        Application.Quit();
        Debug.Log("Quit");
    }
}
