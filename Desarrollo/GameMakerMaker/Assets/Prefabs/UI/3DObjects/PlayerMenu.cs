using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMenu : MonoBehaviour, IPointerClickHandler {

    [Header("Timers")]
    public float TimeToAchoos;
    float time = 0;

    [HideInInspector]
    public SoundController anim;
    bool interacting;

    public PlayerMenu shadow;

    void Start()
    {
        anim = GetComponent<SoundController>();
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interacting)
        {
            if (shadow != null)
            {
                shadow.OnPointerClick(eventData);
            }
            interacting = true;
            anim.SetTrigger(Constantes.ANIMATION_PLAYER_CLICK);
            StartCoroutine(Achoos());
        }
    }
}
