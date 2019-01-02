using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(SoundController))]
public class OvenBehaviour : Interactuable
{
    public Image progress;
    public Image danger;
    // Use this for initialization

    PropBehaviour CookingProp;
    WaitForEndOfFrame wait;
    float time = 0;

    public float brokenTime;
    float currentBrokenTime = 0;

    public float explosionTime;
    float currentExplosionTime = 0;
    Coroutine explosion;

    [HideInInspector]
    public SoundController anim;
    public TextMeshProUGUI textDebug;

    public float TimeToCook;

    public GameObject Indicator;

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
            ovenState = State.Coocking;
            textDebug.text = ("Por fin haces algo bien");
            while (time < TimeToCook)
            {
                time += Time.deltaTime;
                progress.fillAmount = (time / TimeToCook);
                yield return wait;
            }
            CookingProp.TasksCompleted++;
            textDebug.text = ("Seguro que ha salido amorfo");
            progress.color = new Color(0, 1, 0);
            task.Complete = true;
            time = 0;
            ovenState = State.Prepare;
            explosion = StartCoroutine(Explosion());
            yield return explosion;
        }
        else
        {
            anim.SetTrigger(Constantes.ANIMATION_OVEN_BREAK);
            ovenState = State.Broken;
            
            textDebug.text = ("Ya la has cagado");
            progress.color = new Color(1, 0, 0);
            while (currentBrokenTime < brokenTime)
            {
                currentBrokenTime += Time.deltaTime;
                progress.fillAmount = (currentBrokenTime / brokenTime);
                yield return wait;
            }
            textDebug.text = ("Aprende de tus errores, tonto");
            progress.fillAmount = 0;
            progress.color = new Color(1, 1, 1);
            currentBrokenTime = 0;
            anim.SetTrigger(Constantes.ANIMATION_OVEN_FIXED);
            GameManager.Instance.StoreProp(CookingProp.gameObject);
            ovenState = State.Empty;
        }
    }

    IEnumerator Explosion()
    {
        while (currentExplosionTime/explosionTime < 0.25f)
        {
            currentExplosionTime += Time.deltaTime;
            yield return wait;
        }
        textDebug.text = ("Corre Gilipollas");
        progress.fillAmount = 0;
        progress.color = new Color(1, 1, 1);
        danger.gameObject.SetActive(true);
        while (currentExplosionTime / explosionTime < 1.1f)
        {
            currentExplosionTime += Time.deltaTime;
            yield return wait;
        }
        anim.SetTrigger(Constantes.ANIMATION_OVEN_BREAK);
        currentExplosionTime = 0;
        ovenState = State.Broken;
        danger.gameObject.SetActive(false);
        textDebug.text = ("Se te ha quemado tonto");
        progress.color = new Color(1, 0, 0);
        while (currentBrokenTime < brokenTime)
        {
            currentBrokenTime += Time.deltaTime;
            progress.fillAmount = (currentBrokenTime / brokenTime);
            yield return wait;
        }
        textDebug.text = ("Todo te sale mal");
        progress.fillAmount = 0;
        progress.color = new Color(1, 1, 1);
        currentBrokenTime = 0;
        anim.SetTrigger(Constantes.ANIMATION_OVEN_FIXED);
        GameManager.Instance.StoreProp(CookingProp.gameObject);
        ovenState = State.Empty;
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
