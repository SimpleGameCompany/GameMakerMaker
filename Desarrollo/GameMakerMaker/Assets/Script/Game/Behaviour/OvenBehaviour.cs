using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(SoundController))]
public class OvenBehaviour : Interactuable
{
    [Header("UI")]
    public Image progress;
    public Image danger;
    public GameObject Indicator;
    public GameObject RigObject;
    public Renderer render;
    public Material RenderMaterial;
    public Material Highlight;



    [Header("Timers")]
    public float brokenTime;
    float currentBrokenTime = 0;
    public float explosionTime;
    float currentExplosionTime = 0;
    Coroutine explosion;
    public float TimeToCook;
    float time = 0;

    [Header("ParticleSystem")]
    public ParticleSystem cooking;
    public ParticleSystem explode;
    //public ParticleSystem broken;

    PropBehaviour CookingProp;
    WaitForEndOfFrame wait;

    [HideInInspector]
    public SoundController anim;

    public enum OvenType
    {
        Magic,
        Iron,
        Texture
    }
    enum State
    {
        Empty,
        Coocking,
        Prepare,
        Wasted,
        Broken
    }

    State ovenState;


    public OvenType Type;
    void Start()
    {
        render = RigObject.GetComponentInChildren<Renderer>();
        RenderMaterial = render.material;
        cooking.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        wait = new WaitForEndOfFrame();
        ovenState = State.Empty;
        anim = GetComponent<SoundController>();
        Indicator = (from x in GetComponentsInChildren<Transform>() where x.CompareTag(Constantes.TAG_INDICATOR) select x.gameObject).FirstOrDefault();
        Indicator.SetActive(false);
    }



    public override bool PreAction(PlayerController player)
    {
        switch (ovenState)
        {
            case State.Empty:
                return player.PickedObjet != null && base.PreAction(player);
            case State.Coocking:
                return false;
            case State.Prepare:          
                return player.PickedObjet == null && base.PreAction(player);
            case State.Wasted:
                return false;
            case State.Broken:
                return false;
            default:
                return true;
               
        }
        
    }

    public override void PostAction(PlayerController player)
    {
        base.PostAction(player);
        switch (ovenState)
        {
            case State.Empty:
                player.interacting = true;
                player.anim.SetTrigger(Constantes.ANIMATION_PLAYER_DROP_OBJECT);
                anim.SetTrigger(Constantes.ANIMATION_OVEN_GET_OBJECT);
                break;
            case State.Coocking:
                break;
            case State.Prepare:
                player.interacting = true;
                anim.SetTrigger(Constantes.ANIMATION_OVEN_DROP_OBJECT);
                player.anim.SetTrigger(Constantes.ANIMATION_PLAYER_PICK);
                progress.fillAmount = 0;
                progress.color = new Color(1, 1, 1);
                time = 0;
                StopCoroutine(explosion);
                danger.gameObject.SetActive(false);
                currentExplosionTime = 0;
                break;
            case State.Wasted:
                break;
            case State.Broken:
                break;
        }
    }


    IEnumerator Cooking()
    {
        OvenInstruction task = (from x in CookingProp.recipe.Tasks where x.Type == this.Type && !x.Complete  select x ).FirstOrDefault();
        //Debug.Log(task.Complete);
        if (task != null) {
            cooking.Play(true);
            anim.SetTrigger(Constantes.ANIMATION_OVEN_COOK);
            ovenState = State.Coocking;
            while (time < TimeToCook)
            {
                time += Time.deltaTime;
                progress.fillAmount = (time / TimeToCook);
                yield return wait;
            }
            CookingProp.TasksCompleted++;
            progress.color = new Color(0, 1, 0);
            task.Complete = true;
            time = 0;
            ovenState = State.Prepare;
            anim.SetTrigger(Constantes.ANIMATION_OVEN_COOK_END);
            explosion = StartCoroutine(Explosion());
            cooking.Stop(true,ParticleSystemStopBehavior.StopEmitting);
            yield return explosion;
        }
        else
        {
            ovenState = State.Broken;
            anim.SetTrigger(Constantes.ANIMATION_OVEN_BREAK);
            explode.gameObject.SetActive(true);
            //broken.Play(true);
            progress.color = new Color(1, 0, 0);
            while (currentBrokenTime < brokenTime)
            {
                currentBrokenTime += Time.deltaTime;
                progress.fillAmount = (currentBrokenTime / brokenTime);
                yield return wait;
            }
            progress.fillAmount = 0;
            progress.color = new Color(1, 1, 1);
            currentBrokenTime = 0;
            anim.SetTrigger(Constantes.ANIMATION_OVEN_FIXED);
            GameManager.Instance.StoreProp(CookingProp.gameObject);
            ovenState = State.Empty;
            //broken.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    IEnumerator Explosion()
    {
        cooking.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        while (currentExplosionTime/explosionTime < 0.25f)
        {
            currentExplosionTime += Time.deltaTime;
            yield return wait;
        }
        anim.SetTrigger("Burning");
        progress.fillAmount = 0;
        progress.color = new Color(1, 1, 1);
        danger.gameObject.SetActive(true);
        while (currentExplosionTime / explosionTime < 1.1f)
        {
            currentExplosionTime += Time.deltaTime;
            yield return wait;
        }
        ovenState = State.Broken;
        explode.gameObject.SetActive(true);
        //broken.Play(true);
        anim.SetTrigger(Constantes.ANIMATION_OVEN_BREAK);
        currentExplosionTime = 0;
        danger.gameObject.SetActive(false);
        progress.color = new Color(1, 0, 0);
        while (currentBrokenTime < brokenTime)
        {
            currentBrokenTime += Time.deltaTime;
            progress.fillAmount = (currentBrokenTime / brokenTime);
            yield return wait;
        }
        progress.fillAmount = 0;
        progress.color = new Color(1, 1, 1);
        currentBrokenTime = 0;
        anim.SetTrigger(Constantes.ANIMATION_OVEN_FIXED);
        GameManager.Instance.StoreProp(CookingProp.gameObject);
        ovenState = State.Empty;
        //broken.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    public override void PostActionAnim(PlayerController player)
    {
        switch (ovenState)
        {
            case State.Empty:
                CookingProp = player.PickedObjet;
                CookingProp.GetComponentInChildren<Collider>().enabled = false;
                player.PickedObjet = null;
                CookingProp.transform.SetParent(transform);
                CookingProp.transform.localPosition = Vector3.zero;
                CookingProp.anim.SetTrigger(Constantes.ANIMATION_PROP_SCALEDOWN);
                StartCoroutine(Cooking());
                player.interacting = false;
                break;
            case State.Prepare:
                player.PickedObjet = CookingProp;
                CookingProp.GetComponentInChildren<Collider>().enabled = true;
                CookingProp.transform.SetParent(player.grabPoint);
                CookingProp = null;
                player.PickedObjet.transform.localPosition = Vector3.zero;
                player.PickedObjet.anim.SetTrigger(Constantes.ANIMATION_PROP_SCALEUP);
                ovenState = State.Empty;
                player.interacting = false;
                break;
        }
    }
}
