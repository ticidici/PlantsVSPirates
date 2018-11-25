using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public Text bufferDisplay;
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
        if (!string.IsNullOrEmpty(Input.inputString) && !GameObject.Find("GameManager").GetComponent<GameManager>().getGameOver()) {
            foreach (char c in Input.inputString) {
                buffer = string.Concat(buffer, c);
            }
            bufferDisplay.text = buffer;
            cloudManager.CheckInputPlayer(buffer);
        }
    }

    public void EmptyBuffer() {
        buffer = string.Empty;
        StartCoroutine(EmptyBufferDisplay());
    }

    public void StartRecordingInput() {
        isRecording = true;
    }

    public void StopRecordingInput() {
        isRecording = false;
    }

    private IEnumerator EmptyBufferDisplay() {
        yield return new WaitForSeconds(0.1f);
        bufferDisplay.text = string.Empty;
    }
}
