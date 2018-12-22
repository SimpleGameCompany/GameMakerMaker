using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameManager.Instance.StartLevelFromCourutine();
	}
	
}
