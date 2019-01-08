using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    
    private static MusicController _instance;


    public AudioClip[] clips;
    private Dictionary<string, int> clipsID;
    private AudioSource audioSource;
    private float firstVolume;
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
        foreach(var e in FindObjectsOfType<AudioSource>())
        {
            e.mute = true;
        }
    }

    public void UnMute()
    {
        Volume = firstVolume;
        foreach (var e in FindObjectsOfType<AudioSource>())
        {
            e.mute = false;
        }
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
        if (Instance != null)
        {

            Destroy(gameObject);
        }
        else
        {
            
            DontDestroyOnLoad(gameObject);
            _instance = this;
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = clips[0];
            audioSource.loop = true;
            audioSource.Play();
            firstVolume = Volume;
            clipsID = new Dictionary<string, int>();
            for (int i = 0; i < clips.Length; i++)
            {
                clipsID.Add(clips[i].name, i);
            }
            //Volume = 1;
        }
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

    public void MuteOtherSounds()
    {
        foreach (var e in FindObjectsOfType<AudioSource>())
        {
            e.mute = true;
        }

        audioSource.mute = false;

    }
    public void UnMuteInGame()
    {

        foreach (var e in FindObjectsOfType<AudioSource>())
        {
            e.mute = false;
        }
    }
	

}
