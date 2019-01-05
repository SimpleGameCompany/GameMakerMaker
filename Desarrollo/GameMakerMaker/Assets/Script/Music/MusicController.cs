using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    
    private static MusicController _instance;


    public AudioClip[] clips;
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

    public float Volume { get { return audioSource.volume; } set { audioSource.volume = value; } }
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

    public void Mute()
    {
        Volume = 0;
    }

    public void UnMute()
    {
        Volume = 1;
    }


    public void MuteButton()
    {
        if (Volume > 0)
        {
            Mute();
        }
        else
        {
            UnMute();
        }
    }

    private void Awake () {
        DontDestroyOnLoad(gameObject);
        _instance = this;
        audioSource = GetComponent<AudioSource>();
        clipsID = new Dictionary<string, int>();
        for(int i = 0; i<clips.Length; i++)
        {
            clipsID.Add(clips[i].name, i);
        }
        Volume = 1;
	}

    public void PlaySong(string clipName)
    {
        int n;
        if(clipsID.TryGetValue(clipName,out n))
        {
            //TODO Habria que hacer que hiciese el mixer y demas bien.
            audioSource.Stop();
            audioSource.clip = clips[n];

            audioSource.Play();
        }
    }

    
	

}
