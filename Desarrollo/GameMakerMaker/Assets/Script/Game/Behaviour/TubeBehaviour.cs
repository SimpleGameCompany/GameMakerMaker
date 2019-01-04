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
    SoundController anim;
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
        anim.SetTrigger(Constantes.ANIMATION_TUBE_FIXED);
        broken = false;
    }

    // Use this for initialization
    void Start () {
        broken = false;
        wait = new WaitForEndOfFrame();
        anim = GetComponent<SoundController>();
	}

    public override void PostActionAnim(PlayerController player)
    {
        anim.SetTrigger(Constantes.ANIMATION_OVEN_GET_OBJECT);
        base.PostAction(player);
        if (player.PickedObjet.world == world && player.PickedObjet.Completed)
        {
            anim.SetTrigger(Constantes.ANIMATION_OVEN_DROP_OBJECT);
            ScoreController.Instance.scoreNumber += player.PickedObjet.recipe.score;
            ScoreController.Instance.Score += 1; 
        }
        else
        {
            anim.SetTrigger(Constantes.ANIMATION_OVEN_BREAK);
            broken = true;
            Debug.Log("roto");
            StartCoroutine(Reparing());
        }

        player.PickedObjet.transform.parent = this.transform;
        player.PickedObjet.transform.localPosition = Vector3.zero;
        player.PickedObjet.transform.rotation = Quaternion.Euler(0, 0, 0);
        GameManager.Instance.StoreProp(player.PickedObjet.gameObject);
        player.PickedObjet = null;
        player.interacting = false;
    }
}
