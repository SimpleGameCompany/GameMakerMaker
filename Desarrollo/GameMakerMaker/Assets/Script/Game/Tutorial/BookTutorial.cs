using UnityEngine;
using System.Collections;
using Polyglot;

public class BookTutorial : BookBehaviour
{
    public LocalizedTextMeshProUGUI text;
    public Light l;
    private void Awake()
    {
        l = transform.parent.GetComponentInChildren<Light>();
        l.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }


    public override void PostAction(PlayerController player)
    {
        TexturizadorTutorial.instance.text.gameObject.SetActive(true);
        TexturizadorTutorial.instance.text.Key = Constantes.KEY_OVEN_GOTO;
        TexturizadorTutorial.instance.GetComponentInChildren<Light>(true).gameObject.SetActive(true);
        text.gameObject.SetActive(false);
        l.gameObject.SetActive(false);
        base.PostAction(player);
    }
}
