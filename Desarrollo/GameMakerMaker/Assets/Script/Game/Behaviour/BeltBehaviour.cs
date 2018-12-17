using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltBehaviour : MonoBehaviour {

    [SerializeField]
    public float speed;
    [SerializeField]
    Vector3 direction;


    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerStay(Collider other)
    {

        other.transform.Translate(speed * Time.deltaTime * direction,Space.World);
    }
}
