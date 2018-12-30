using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCode : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.L))
        {
            if (Input.GetKey(KeyCode.O))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    if (Input.GetKey(KeyCode.D))
                    {
                        PlayerPrefs.SetInt("maxlevel",10);
                        GameManager.Instance.maxlevel = 10;
                    }
                }
            }
        }
	}
}
