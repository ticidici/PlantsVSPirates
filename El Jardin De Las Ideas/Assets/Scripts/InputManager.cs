using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private string buffer = string.Empty;
    public bool isRecording = false;
    public CloudManager cloudManager;
    public static InputManager instance = null;

    public void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

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
