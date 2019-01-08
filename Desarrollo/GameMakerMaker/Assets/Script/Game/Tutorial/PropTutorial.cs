using Polyglot;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PropTutorial : PropBehaviour {



    public LocalizedTextMeshProUGUI text;
    public override void SetNavMeshDestination()
    {
        base.SetNavMeshDestination();

    }

    public override void PostActionAnim(PlayerController player)
    {
        base.PostActionAnim(player);
        TexturizadorTutorial.instance.text.gameObject.SetActive(true);
        TexturizadorTutorial.instance.text.Key = Constantes.KEY_OVEN_GOTO;
        text.gameObject.SetActive(false);
    }


}
