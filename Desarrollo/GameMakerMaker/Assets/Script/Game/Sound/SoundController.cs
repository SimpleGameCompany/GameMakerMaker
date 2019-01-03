using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour  {
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public AudioSource soundDealer;
    public Sound[] sounds;
    public List<string> TriggerName;


    public void Awake()
    {
        anim = GetComponent<Animator>();
        if(anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }
        soundDealer = GetComponent<AudioSource>();
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].name = TriggerName[i];
        }
    }

    public void SetTrigger(string Trigger)
    {

        anim.SetTrigger(Trigger);
        SetSound(Trigger);
        
    }

    public void SetFloat(string name,float value)
    {
        anim.SetFloat(name, value);
        SetSound(name);
    }

    public void SetBool(string name, bool value)
    {
        anim.SetBool(name, value);
        SetSound(name);
    }

    public void SetSound(string name)
    {
        soundDealer.Stop();

        if (TriggerName.Contains(name))
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            soundDealer.loop = s.loop;
            soundDealer.volume = s.volume;
            soundDealer.pitch = s.pitch;
            soundDealer.PlayOneShot(s.clip);
        }
    }


}
