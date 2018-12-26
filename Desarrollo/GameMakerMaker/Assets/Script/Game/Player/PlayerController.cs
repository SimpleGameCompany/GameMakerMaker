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
    public float rotateSpeed = 135;
    public Transform grabPoint;
    Vector3 lastFacing;
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

        Vector3 currentFacing = transform.forward;
        float currentAngularVelocity = Vector3.Angle(currentFacing, lastFacing) / Time.deltaTime; //degrees per second
        lastFacing = currentFacing;

        anim.SetFloat(Constantes.ANIMATION_PLAYER_ANGULARSPEED, currentAngularVelocity);
        anim.SetFloat(Constantes.ANIMATION_PLAYER_SPEED, agent.velocity.magnitude);
    }


    IEnumerator GoToPoint()
    {


        agent.CalculatePath(agent.destination, agent.path);
        yield return new WaitUntil(() => { return !agent.pathPending; });
        Vector3 PointDir = agent.path.corners[1] - transform.position;
        Vector3 preforward = transform.forward;
        agent.isStopped = true;
        float t = Mathf.Abs(Vector3.Angle(PointDir, transform.forward));
        anim.SetBool(Constantes.ANIMATION_PLAYER_ROTATE, true);

        while (t > 5)
        {

            transform.forward = Vector3.RotateTowards(transform.forward, PointDir, Mathf.Deg2Rad * rotateSpeed * Time.deltaTime, 0.0f);
            t = Mathf.Abs(Vector3.Angle(PointDir,transform.forward));
            yield return null;

        }



        anim.SetBool(Constantes.ANIMATION_PLAYER_ROTATE, false);
        agent.isStopped = false;
        

        while (agent.remainingDistance>agent.stoppingDistance)
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

    public void ActionAnim()
    {
        actualTask.PostActionAnim(this);
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
