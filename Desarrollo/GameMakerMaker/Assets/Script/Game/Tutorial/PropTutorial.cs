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
        BookTutorial b = FindObjectOfType<BookTutorial>();
        b.text.gameObject.SetActive(true);
        b.l.gameObject.SetActive(true);
        text.gameObject.SetActive(false);
    }


}
