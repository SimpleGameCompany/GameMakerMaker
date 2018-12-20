using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour {

    NavMeshAgent agent;
    Interactuable actualTask;
    [HideInInspector]
    public  PropBehaviour PickedObjet;
    private NavMeshHit navMeshHit;
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
                        if (NavMesh.SamplePosition(hit.point, out navMeshHit, 10, NavMesh.AllAreas))
                        {
                            agent.SetDestination(navMeshHit.position);
                            StartCoroutine(GoToPoint());
                        }
                    }
                }
            }
        }
    }


    IEnumerator GoToPoint()
    {
        while (Vector3.Distance(transform.position,agent.destination)>1)
        {
            //Debug.Log(Vector3.Distance(transform.position, agent.destination));
            yield return null;
        }
        actualTask.PostAction(this);
    }
}
