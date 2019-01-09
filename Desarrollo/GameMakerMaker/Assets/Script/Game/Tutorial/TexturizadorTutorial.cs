using Polyglot;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TexturizadorTutorial : OvenBehaviour {

    public static TexturizadorTutorial instance;
    public LocalizedTextMeshProUGUI text;
     public new void Start()
    {
        base.Start();
        instance = this;
        text.gameObject.SetActive(false);
    }

    protected override void AfterCook()
    {
        text.Key = Constantes.KEY_OVEN_TAKE;
    }

    public override IEnumerator Cooking()
    {
        GetComponentInChildren<Light>().gameObject.SetActive(false);
        text.gameObject.SetActive(true);
        text.Key = Constantes.KEY_OVEN_COOKING;
        return base.Cooking();
    }

    protected override void AfterPick()
    {
        
        text.gameObject.SetActive(false);
        LocalizedTextMeshProUGUI l =  FindObjectOfType<TubeBehaviour>().GetComponentInChildren<LocalizedTextMeshProUGUI>(true);
        l.gameObject.SetActive(true);
        l.transform.parent.parent.GetComponentInChildren<Light>(true).gameObject.SetActive(true);
    }


}
