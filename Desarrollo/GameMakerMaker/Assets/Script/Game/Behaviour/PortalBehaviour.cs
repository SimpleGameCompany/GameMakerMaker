using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehaviour : TubeBehaviour {

    public ParticleSystem entrega;

    public new void PostActionAnim(PlayerController player)
    {
        entrega.gameObject.SetActive(true);
        base.PostActionAnim(player);
    }
}
