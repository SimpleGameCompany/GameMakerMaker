using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour  {

    public Animator anim;
    public AudioSource soundDealer;
    public List<AudioClip> clips;
    public List<string> TriggerName;


    public void Start()
    {
        anim = GetComponent<Animator>();
        soundDealer = GetComponent<AudioSource>();

    }

    public void SetTrigger(string Trigger)
    {

        anim.SetTrigger(Trigger);
        SetSound(Trigger);
        
    }

    public void SetFloat(string name,float value)
    {
        anim.SetFloat(name, value);
    }

    public void SetBool(string name, bool value)
    {
        anim.SetBool(name, value);
        SetSound(name);
    }

    public void SetSound(string name)
    {
        soundDealer.Stop();
        int t = TriggerName.IndexOf(name);
        if (t != -1)
        {
            AudioClip clipToPlay = clips[t];
            soundDealer.PlayOneShot(clipToPlay);
        }
    }


}
