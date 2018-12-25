using UnityEngine;
using System.Collections;

public class PaperBehaviour : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        PropBehaviour p = other.GetComponent<PropBehaviour>();

        if(p == null)
        {
            p = other.transform.GetComponentInParent<PropBehaviour>();
        }


        if(p != null)
        {

            p.anim.SetTrigger(Constantes.ANIMATION_PROP_DESTROY);
        }
        
    }
}
