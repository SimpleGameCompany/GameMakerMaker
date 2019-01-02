using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {


    private static MusicController _instance;


    private AudioClip[] clips;
    private Dictionary<string, int> clipsID;
    private AudioSource audioSource;

    public static MusicController Instance
    {
        get
        {
            return _instance;
        }

        private set
        {
            _instance = value;
        }
    }

    public float Volume { get { return audioSource.volume; } set { audioSource.volume = Volume; } }
    public void Stop()
    {
        audioSource.Stop();
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Resume()
    {
        audioSource.UnPause();
    }

    void Start () {
        _instance = this;
        audioSource = GetComponent<AudioSource>();
        clipsID = new Dictionary<string, int>();
        for(int i = 0; i<clips.Length; i++)
        {
            clipsID.Add(clips[i].name, i);
        }
	}

    public void PlaySong(string clipName)
    {
        int n;
        if(clipsID.TryGetValue(clipName,out n))
        {
            //TODO Habria que hacer que hiciese el mixer y demas bien.
            audioSource.PlayOneShot(clips[n]);
        }
    }

    
	

}
