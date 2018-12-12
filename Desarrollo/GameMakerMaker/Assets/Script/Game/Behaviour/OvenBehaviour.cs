using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class OvenBehaviour : Interactuable
{
    public Image progress;
    // Use this for initialization

    PropBehaviour CookingProp;
    WaitForEndOfFrame wait;
    float time = 0;
    public float brokenTime;
    float currentBrokenTime = 0;
    public TextMeshProUGUI textDebug;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool PreAction(PlayerController player)
    {
        switch (ovenState)
        {
            case State.Empty:
                return player.PickedObjet != null;
            case State.Coocking:
                return false;
            case State.Prepare:
                return true;
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
        switch (ovenState)
        {
            case State.Empty:
                CookingProp = player.PickedObjet;
                player.PickedObjet = null;
                CookingProp.transform.SetParent(transform);
                CookingProp.transform.localPosition = Vector3.zero;
                StartCoroutine(Cooking());
                break;
            case State.Coocking:
                break;
            case State.Prepare:
                player.PickedObjet = CookingProp;
                CookingProp.transform.SetParent(player.transform);
                CookingProp = null;
                player.PickedObjet.transform.localPosition = Vector3.zero;
                progress.fillAmount = 0;
                progress.color = new Color(1, 1, 1);
                time = 0;
                textDebug.text = "";
                ovenState = State.Empty;
                StopCoroutine(Cooking());              
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
            while (time < task.Time)
            {
                time += Time.deltaTime;
                progress.fillAmount = (time / task.Time);
                yield return wait;
            }
            textDebug.text = ("Seguro que ha salido amorfo");
            progress.color = new Color(0, 1, 0);
            task.Complete = true;
            ovenState = State.Prepare;
        }
        else
        {
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
            ovenState = State.Empty;
        }
    }


}
