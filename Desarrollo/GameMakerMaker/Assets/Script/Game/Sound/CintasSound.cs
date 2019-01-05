using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CintasSound : MonoBehaviour {

    SoundController anim;


	// Use this for initialization
	void Start () {
        anim = GetComponent<SoundController>();

        StartCoroutine(Init());
	}
	
    IEnumerator Init()
    {
        anim.SetTrigger("Init");
        float i = 0;
        while (i < 4f)
        {
            i += Time.deltaTime;
            yield return null;
        }
        anim.SetTrigger("Loop");
    }
}
