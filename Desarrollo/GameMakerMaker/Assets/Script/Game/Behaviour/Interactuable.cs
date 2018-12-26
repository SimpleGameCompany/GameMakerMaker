using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Interactuable : MonoBehaviour {


    public Image actionIcon;

    public abstract bool PreAction(PlayerController player);
    public abstract void PostAction(PlayerController player);
    public abstract void PostActionAnim(PlayerController player);
    public virtual bool UpdatePosition(PlayerController player)
    {
        return true;
    }

    public virtual bool HasToStare()
    {
        return true;
    }

}
