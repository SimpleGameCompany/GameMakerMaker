using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeballMenu : MonoBehaviour {

    public Transform Inicio;
    public Vector3 forward;
    SoundController anim;

    public void Start()
    {
        forward = transform.InverseTransformVector(transform.forward);
        anim = GetComponent<SoundController>();
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(forward * 0.2f * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other)
    {
        anim.SetTrigger(Constantes.ANIMATION_PROP_DESTROY);
        transform.position = Inicio.position;
        anim.SetTrigger(Constantes.ANIMATION_PROP_SPAWN);
    }
}
