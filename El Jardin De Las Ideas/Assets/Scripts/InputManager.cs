using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private string buffer = string.Empty;
    public bool isRecording = false;
    public Nube cloudManager;

    void Update() {
        if (!string.IsNullOrEmpty(Input.inputString)) {
            foreach (char c in Input.inputString) {
                buffer = string.Concat(buffer, c);
            }

            cloudManager.CheckInputPlayer(buffer);
        }
    }

    public void EmptyBuffer() {
        buffer = string.Empty;
    }

    public void StartRecordingInput() {
        isRecording = true;
    }

    public void StopRecordingInput() {
        isRecording = false;
    }
}
