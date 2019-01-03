using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenu : MonoBehaviour {

    [Header("Timers")]
    public float TimeToAchoos;
    float time = 0;

    [HideInInspector]
    public SoundController anim;
    bool interacting;


    void Start()
    {
        anim = GetComponent<SoundController>();
    }

    private void OnMouseDown()
    {
        if (!interacting)
        {
            interacting = true;
            anim.SetTrigger(Constantes.ANIMATION_PLAYER_CLICK);
            StartCoroutine(Achoos());
        }
    }

    IEnumerator Achoos()
    {
        while (time < TimeToAchoos)
        {
            time += Time.deltaTime;
            yield return null;
        }
        time = 0;

        interacting = false;
    }
}
