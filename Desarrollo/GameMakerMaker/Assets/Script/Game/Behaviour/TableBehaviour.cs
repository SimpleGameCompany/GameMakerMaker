﻿using System.Collections;
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
                return player.PickedObjet != null && base.PreAction(player);
            case State.Full:
                return player.PickedObjet == null && base.PreAction(player);
            default:
                return false;
        }
    }

    public override void PostAction(PlayerController player)
    {
        base.PostAction(player);
        switch (tableState)
        {
            case State.Empty:
                player.anim.SetTrigger(Constantes.ANIMATION_PLAYER_DROP_OBJECT);
                player.interacting = true;
                break;
            case State.Full:
                player.anim.SetTrigger(Constantes.ANIMATION_PLAYER_PICK);
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
            case State.Empty:
                Prop = player.PickedObjet;
                player.PickedObjet = null;
                Prop.transform.SetParent(transform);
                Prop.transform.localPosition = Vector3.zero;
                player.interacting = false;
                tableState = State.Full;
                break;
            case State.Full:
                player.PickedObjet = Prop;
                Prop.transform.SetParent(player.grabPoint);
                Prop = null;
                player.PickedObjet.transform.localPosition = Vector3.zero;
                player.interacting = false;
                tableState = State.Empty;
                break;
            default:
                break;
        }
    }
}
