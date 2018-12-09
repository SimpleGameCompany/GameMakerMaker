using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour {

    NavMeshAgent agent;
    Interactuable actualTask;
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                Interactuable inter = hit.collider.gameObject.GetComponent<Interactuable>();
                
                if (inter != null)
                {
                    actualTask = inter;
                    StopCoroutine(GoToPoint());
                    if (inter.PreAction(this)) {              
                        agent.SetDestination(hit.point);                      
                        StartCoroutine(GoToPoint());
                    }
                }
            }
        }
    }


    IEnumerator GoToPoint()
    {
        
        while (Vector3.Distance(transform.position,agent.destination)>1)
        {
           
            yield return null;
        }
        actualTask.PostAction(this);
    }
}
