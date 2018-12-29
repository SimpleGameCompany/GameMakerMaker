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
        player.anim.SetTrigger(Constantes.ANIMATION_PLAYER_DROP_OBJECT);
        player.interacting = true;
    }

    public override bool PreAction(PlayerController player)
    {
        if((player.PickedObjet != null && !broken))
        {
            return base.PreAction(player);
            
        }
        return false;  
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

    public override void PostActionAnim(PlayerController player)
    {
        base.PostAction(player);
        if (player.PickedObjet.world == world && player.PickedObjet.Completed)
        {
            ScoreController.Instance.scoreNumber += player.PickedObjet.recipe.score;
            ScoreController.Instance.Score += 1; 
        }
        else
        {
            broken = true;
            Debug.Log("roto");
            StartCoroutine(Reparing());
        }

        GameManager.Instance.StoreProp(player.PickedObjet.gameObject);
        player.PickedObjet = null;
        player.interacting = false;
    }
}
