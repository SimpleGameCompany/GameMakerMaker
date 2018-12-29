using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Interactuable : MonoBehaviour {


    public Image actionIcon;

    public virtual bool PreAction(PlayerController player) {
        player.MarkObject.SetActive(true);
        player.MarkObject.transform.position = transform.position;
        player.MarkObject.transform.SetParent(transform);
        return true;
    }
    public virtual void PostAction(PlayerController player) {
        player.MarkObject.transform.SetParent(player.transform);
        player.MarkObject.transform.localScale = Vector3.one;
        player.MarkObject.transform.rotation = Quaternion.identity;
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
