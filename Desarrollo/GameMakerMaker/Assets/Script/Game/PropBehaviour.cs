using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropBehaviour : Interactuable {


    public Recipe recipe;
    public override void PostAction(PlayerController player)
    {
        player.PickedObjet = this;
        gameObject.transform.SetParent(player.transform);
        gameObject.transform.localPosition = Vector3.zero;
    }

    public override bool PreAction(PlayerController player)
    {
        if (player.PickedObjet == null)
        {
            return true;
        }
        else
            return false;
    }


    private void Start()
    {
        recipe = Instantiate(recipe) as Recipe;
    }
}
