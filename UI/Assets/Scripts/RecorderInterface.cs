using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface RecorderInterface {
    void StartRecording(int projectNo, int gestureNo, int subjectNo, int indexOfSubjectGesture);
    void StopRecording();
}
