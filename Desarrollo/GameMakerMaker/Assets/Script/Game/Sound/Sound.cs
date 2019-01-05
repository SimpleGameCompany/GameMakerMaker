using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound {

    [HideInInspector]
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1.0f;
    [Range(.1f, 3f)]
    public float pitch = 1.0f;

    public bool loop;
}
