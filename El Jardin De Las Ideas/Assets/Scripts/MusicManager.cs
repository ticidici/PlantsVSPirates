using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    [FMODUnity.EventRef]
    public string MainThemeEvent;
    FMOD.Studio.EventInstance Music;

    void Start () {
        Music = FMODUnity.RuntimeManager.CreateInstance(MainThemeEvent);
        Music.start();
    }
}
