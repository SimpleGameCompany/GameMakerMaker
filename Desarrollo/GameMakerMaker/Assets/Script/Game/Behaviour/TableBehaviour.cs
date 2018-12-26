using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBehaviour : Interactuable {

    PropBehaviour Prop;

    enum State
    {
        Empty,
        Full
    }

    State tableState;

    // Use this for initialization
    void Start () {
        tableState = State.Empty;
	}

    // Update is called once per frame
    void Update()
    {

    }

    public override bool PreAction(PlayerController player)
    {
        switch (tableState)
        {
            case State.Empty:
                return player.PickedObjet != null;
            case State.Full:
                return player.PickedObjet == null;
            default:
                return false;
        }
    }

    public override void PostAction(PlayerController player)
    {
        switch (tableState)
        {
            case State.Empty:
                player.anim.SetTrigger(Constantes.ANIMATION_PLAYER_DROP_OBJECT);
                tableState = State.Full;
                player.interacting = true;
                break;
            case State.Full:
                player.anim.SetTrigger(Constantes.ANIMATION_PLAYER_PICK);
                tableState = State.Empty;
                player.interacting = true;
                break;
            default:
                break;
        }
    }

    public void Remove()
    {
        Prop = null;
        tableState = State.Empty;
    }

    public override void PostActionAnim(PlayerController player)
    {
        switch (tableState)
        {
            case State.Full:
                Prop = player.PickedObjet;
                player.PickedObjet = null;
                Prop.transform.SetParent(transform);
                Prop.transform.localPosition = Vector3.zero;
                player.interacting = false;
                break;
            case State.Empty:
                player.PickedObjet = Prop;
                Prop.transform.SetParent(player.grabPoint);
                Prop = null;
                player.PickedObjet.transform.localPosition = Vector3.zero;
                player.interacting = false;
                break;
            default:
                break;
        }
    }
}
