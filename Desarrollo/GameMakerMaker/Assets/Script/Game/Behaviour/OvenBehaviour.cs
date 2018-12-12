using UnityEngine;
using System.Collections;
using System.Linq;
public class OvenBehaviour : Interactuable
{

    // Use this for initialization

    PropBehaviour CookingProp;
    WaitForEndOfFrame waiter;
    float time = 0;
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
        waiter = new WaitForEndOfFrame();
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

                return player.PickedObjet == null;
                break;
            case State.Coocking:
                return false;
                break;
            case State.Prepare:
                return true;
                break;
            case State.Wasted:
                return false;
                break;
            case State.Broken:
                return false;
                break;
            default:
                return true;
                break;
               
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
        Debug.Log(task.Complete);
        if (task != null) {
            ovenState = State.Coocking;
            while (time < task.Time)
            {
                time += Time.deltaTime;   
                yield return waiter;
            }
            Debug.Log("Cocinado");
            task.Complete = true;
            ovenState = State.Prepare;

        }
        else
        {
            ovenState = State.Broken;
        }
    }


}
