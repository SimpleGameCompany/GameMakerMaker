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
                Prop = player.PickedObjet;
                player.PickedObjet = null;
                Prop.transform.SetParent(transform);
                Prop.transform.localPosition = Vector3.zero;
                tableState = State.Full;
                break;
            case State.Full:
                player.PickedObjet = Prop;
                Prop.transform.SetParent(player.transform);
                Prop = null;
                player.PickedObjet.transform.localPosition = Vector3.zero;
                tableState = State.Empty;
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
}
