using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TubeBehaviour : Interactuable {

    public Image progress;
    private bool broken;
    [SerializeField]
    private PropBehaviour.PropWorld world;
    public float brokenTime;
    float currentBrokenTime;
    WaitForEndOfFrame wait;


    public override void PostAction(PlayerController player)
    {
        if(player.PickedObjet.world == world && player.PickedObjet.Completed)
        {
            ScoreController.Instance.Score += player.PickedObjet.recipe.score;
        }
        else
        {
            broken = true;
            Debug.Log("roto");
            StartCoroutine(Reparing());
        }
        player.anim.SetTrigger(Constantes.ANIMATION_PLAYER_DROP_OBJECT);
        GameManager.Instance.StoreProp(player.PickedObjet.gameObject);
        player.PickedObjet = null;

    }

    public override bool PreAction(PlayerController player)
    {
        return (player.PickedObjet != null && !broken);      
    }

    IEnumerator Reparing()
    {
        progress.color = new Color(1, 0, 0);
        while (currentBrokenTime < brokenTime)
        {
            progress.fillAmount = (currentBrokenTime / brokenTime);
            currentBrokenTime += Time.deltaTime;
            yield return wait;
        }
        progress.color = new Color(1, 1, 1);
        progress.fillAmount = 0;
        currentBrokenTime = 0;
        Debug.Log("arreglado");
        broken = false;
    }

    // Use this for initialization
    void Start () {
        broken = false;
	}
	
}
