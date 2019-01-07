using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinecraftBehavior : TubeBehaviour {

    public ParticleSystem entrega;

    public override void PostActionAnim(PlayerController player)
    {
        entrega.gameObject.SetActive(true);
        base.PostActionAnim(player);
    }

}
