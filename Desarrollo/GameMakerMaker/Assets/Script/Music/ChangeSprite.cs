using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour {

    public Sprite mute, unMute;
    public Image button;

    public bool muted = false;

	// Use this for initialization
	void Start () {
        muted = false;
	}
	
    public void Change()
    {
        if (muted)
        {
            MusicController.Instance.MuteButton();
            button.sprite = unMute;
            muted = false;
        }
        else
        {
            MusicController.Instance.MuteButton();
            button.sprite = mute;
            muted = true;
        }
    }
}
