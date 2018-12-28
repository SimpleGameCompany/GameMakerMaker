using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfimWindow : MonoBehaviour {

    public Button[] buttonToDeactivate;

    public void SetVisible()
    {
        this.gameObject.SetActive(true);
        for(int i=0; i<buttonToDeactivate.Length; i++)
        {
            buttonToDeactivate[i].interactable = false;
        }
    }

    public void SetInvisible()
    {
        for (int i = 0; i < buttonToDeactivate.Length; i++)
        {
            buttonToDeactivate[i].interactable = true;
        }
        this.gameObject.SetActive(false);
    }

}
