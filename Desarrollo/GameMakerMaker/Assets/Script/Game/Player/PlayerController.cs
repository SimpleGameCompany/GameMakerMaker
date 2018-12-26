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
    Vector3 lastFacing;

    public Transform ObjectPosition;
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
        agent.isStopped = true;
        yield return StartCoroutine(RotateTo(agent.path.corners[1]));
       
        agent.isStopped = false;
        

        while (Vector3.Distance(transform.position,agent.destination)>0.1)
        {
            if (actualTask.UpdatePosition(this))
            {
                Debug.DrawRay(transform.position + Vector3.up, agent.destination - transform.position, Color.black);
                yield return null;
            }
            else
            {
                StopCoroutine(actionInProcess);
                actionInProcess = null;
            }
        }

        agent.isStopped = true;
        if (actualTask.HasToStare())
        {
            yield return StartCoroutine(RotateTo(actualTask.transform.position));
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
            agent.isStopped = false;
            agent.SetDestination(navMeshHit.position);
            return true;
        }
        return false;
    }

    public void PickUpObject()
    {
        PickedObjet.transform.SetParent(ObjectPosition);

        PickedObjet.transform.localPosition = Vector3.zero;
    }

    IEnumerator RotateTo(Vector3 Point)
    {
       
        Vector3 PointDir = Point - transform.position;
        PointDir = Vector3.ProjectOnPlane(PointDir, Vector3.up);
        Vector3 preforward = transform.forward;  
        float t = Mathf.Abs(Vector3.Angle(PointDir, transform.forward));
        anim.SetBool(Constantes.ANIMATION_PLAYER_ROTATE, true);
        Debug.DrawRay(transform.position+Vector3.up, PointDir,Color.red);
        while (t > 5)
        {

            transform.forward = Vector3.RotateTowards(transform.forward, PointDir, Mathf.Deg2Rad * rotateSpeed * Time.deltaTime, 0.0f);
            t = Mathf.Abs(Vector3.Angle(PointDir, transform.forward));
            Debug.DrawRay(transform.position + Vector3.up, PointDir, Color.red);
            yield return null;

        }



        anim.SetBool(Constantes.ANIMATION_PLAYER_ROTATE, false);
        
    }
}
