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
    private Coroutine actionInProcess;
    [HideInInspector]
    public Animator anim;
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
        Move();
        Vector3 s = agent.transform.InverseTransformDirection(agent.velocity).normalized;
        float speed = s.z;
        float turn = s.x;
        anim.SetFloat(Constantes.ANIMATION_PLAYER_ANGULARSPEED, turn/2);
        anim.SetFloat(Constantes.ANIMATION_PLAYER_SPEED, agent.velocity.magnitude);
    }


    IEnumerator GoToPoint()
    {
        while (Vector3.Distance(transform.position,agent.destination)>1)
        {
            if (actualTask.UpdatePosition(this))
            {
                yield return null;
            }
            else
            {
                StopCoroutine(actionInProcess);
                actionInProcess = null;
            }
        }

        actualTask.PostAction(this);
    }

    void Move()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                Interactuable inter = hit.collider.gameObject.GetComponent<Interactuable>();

                if(inter == null)
                {
                    inter = hit.transform.parent.GetComponent<Interactuable>();
                }

                if (inter != null)
                {
                    actualTask = inter;
                    if (actionInProcess != null)
                    {
                        StopCoroutine(actionInProcess);
                        actionInProcess = null;
                    }
                    if (inter.PreAction(this))
                    {
                        if (SetDestination(hit.point))
                            actionInProcess = StartCoroutine(GoToPoint());
                        
                    }
                }
            }
        }
    }

    public bool SetDestination(Vector3 point)
    {
        if (NavMesh.SamplePosition(point, out navMeshHit, 10, NavMesh.AllAreas))
        {
            agent.SetDestination(navMeshHit.position);
            return true;
        }
        return false;
    }
}
