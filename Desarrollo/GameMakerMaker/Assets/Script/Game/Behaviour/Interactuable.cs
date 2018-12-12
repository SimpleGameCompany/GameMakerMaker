using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Interactuable : MonoBehaviour {


    public Image actionIcon;

    public abstract bool PreAction(PlayerController player);
    public abstract void PostAction(PlayerController player);


}
