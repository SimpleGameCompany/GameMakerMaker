using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Interactuable : MonoBehaviour {


    public Image actionIcon;

    public virtual bool PreAction(PlayerController player) {
        player.MarkObject.SetActive(true);
        player.MarkObject.transform.SetParent(transform);
        player.MarkObject.transform.localPosition = Vector3.zero;
        return true;
    }

    public virtual void PostAction(PlayerController player) {
        player.MarkObject.transform.SetParent(null);
        player.MarkObject.transform.rotation = Quaternion.Euler(0,0,0);
        player.MarkObject.transform.localScale = Vector3.one;
        player.MarkObject.SetActive(false);
    }
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
