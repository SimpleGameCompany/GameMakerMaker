using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeBehaviour : Interactuable {

    private bool broken;
    [SerializeField]
    private PropBehaviour.PropWorld world;


    public override void PostAction(PlayerController player)
    {
        if(player.PickedObjet.world == world)
        {
            ScoreController.Instance.Score += player.PickedObjet.recipe.score;
        }
        else
        {
            broken = true;
        }
        GameManager.Instance.StoreProp(player.PickedObjet.gameObject);
        player.PickedObjet = null;

    }

    public override bool PreAction(PlayerController player)
    {
        return (player.PickedObjet != null && !broken);      
    }

    // Use this for initialization
    void Start () {
        broken = false;
	}
	
}
