using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPage : MonoBehaviour {

	public List<VideoPlayer> CompletedVideos;
    private VideoPlayer[] videos;
    public static VideoPage instance;
    public Texture Default;
    private VideoPlayer ActualVideo;
	void Start () {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            CompletedVideos = new List<VideoPlayer>();
            videos = GetComponentsInChildren<VideoPlayer>();

            foreach(var e in videos)
            {
                e.Prepare();
                StartCoroutine(LoadVideo(e));
            }
        }
        else
        {
            Destroy(gameObject);
        }
	}


    IEnumerator LoadVideo(VideoPlayer player)
    {
        while (!player.isPrepared)
        {
            yield return null;
        }
        Debug.Log("Add video");
        CompletedVideos.Add(player);
    }

    public Texture getRandomVideo()
    {
        
        if(CompletedVideos.Count < 1)
        {
            ActualVideo = null;
            return Default;
        }
        else
        {
            ActualVideo = CompletedVideos[Random.Range(0, CompletedVideos.Count)];
            ActualVideo.Play();
            return ActualVideo.texture;
        }

    }

    public void StopVideo()
    {
        if(ActualVideo != null)
        {
            ActualVideo.Pause();
                   
        }

    }
    
}
