﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TexturizadorMenu : MonoBehaviour, IPointerClickHandler
{
    [Header("Timers")]
    public float TimeToCook;
    float time = 0;

    [Header("ParticleSystem")]
    public ParticleSystem cooking;

    [HideInInspector]
    public SoundController anim;

    bool cook = false;

    void Start()
    {
        cooking.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        anim = GetComponent<SoundController>();
    }

    IEnumerator Cooking()
    {
        cooking.Play(true);
        anim.SetTrigger(Constantes.ANIMATION_OVEN_COOK);
        while (time < TimeToCook)
        {
            time += Time.deltaTime;
            yield return null;
        }
        time = 0;
        anim.SetTrigger(Constantes.ANIMATION_OVEN_COOK_END);
        cooking.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        anim.SetTrigger(Constantes.ANIMATION_OVEN_DROP_OBJECT);
        cook = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!cook)
        {
            cook = true;
            anim.SetTrigger(Constantes.ANIMATION_OVEN_GET_OBJECT);
            StartCoroutine(Cooking());
        }
    }
}
