using UnityEngine;
using System.Collections;

public class PaperBehaviour : MonoBehaviour
{
    SoundController anim;

    public void Start()
    {
        anim = GetComponent<SoundController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PropBehaviour p = other.GetComponent<PropBehaviour>();

        if(p == null)
        {
            p = other.transform.GetComponentInParent<PropBehaviour>();
        }


        if(p != null && p.agent.enabled)
        {

            p.grab = false;
            p.anim.SetTrigger(Constantes.ANIMATION_PROP_DESTROY);
            anim.SetTrigger("Trash");
        }
        
    }
}
