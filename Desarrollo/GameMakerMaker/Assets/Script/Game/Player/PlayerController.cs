using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour {

    public NavMeshAgent agent;
    Interactuable actualTask;

    public GameObject MarkObject;
    public GameObject MarkObjectFloor;
    [HideInInspector]
    public Vector3 click;
    private PropBehaviour prop;
    [HideInInspector]

    System.Random r;

    public PropBehaviour PickedObjet { get { return prop; } set
        {
           if(prop != null)
            {
                prop.DesactiveBehaviour();
            }
            prop = value;
            if(prop != null)
            {
                prop.Activate();
            }
        }
    }
    private NavMeshHit navMeshHit;
    private Coroutine actionInProcess;
    private Coroutine rotationInProcess;
    [HideInInspector]
    public SoundController anim;
    [HideInInspector]
    public bool interacting;
    public float rotateSpeed = 135;
    public Transform grabPoint;
    Vector3 lastFacing;
    public RaycastHit hit;
    [HideInInspector]
    public float ikvalue;

    bool win;
    [HideInInspector]
    public bool endGame;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<SoundController>();
        MarkObject = Instantiate(MarkObject, null);
        MarkObject.SetActive(false);
        MarkObjectFloor = Instantiate(MarkObjectFloor, null);
        MarkObjectFloor.SetActive(false);
        r = new System.Random();
    }
	
	// Update is called once per frame
	void Update () {
        Move();
        Vector3 s = agent.transform.InverseTransformDirection(agent.velocity).normalized;

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

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            if (agent.path.corners.Length > 1)
            {
                yield return StartCoroutine(RotateTo(agent.path.corners[1]));
            }
            else
            {
                yield return StartCoroutine(RotateTo(agent.path.corners[0]));
            }
        }
        agent.isStopped = false;
        

        while (agent.remainingDistance>agent.stoppingDistance)
        {
            if (actualTask.UpdatePosition(this))
            {
                Debug.DrawRay(transform.position + Vector3.up, agent.destination - transform.position, Color.black);
                yield return null;
            }
            else
            {
                MarkObject.transform.SetParent(transform);
                MarkObject.transform.localScale = Vector3.one;
                MarkObject.SetActive(false);
                MarkObjectFloor.transform.rotation = Quaternion.Euler(0, 0, 0);

                MarkObjectFloor.transform.rotation = Quaternion.Euler(0, 0, 0);
                MarkObjectFloor.transform.localScale = Vector3.one;
                MarkObjectFloor.SetActive(false);
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

    public void ActionAnim()
    {
        actualTask.PostActionAnim(this);
    }

    public IEnumerator EndAnim(bool win)
    {
        endGame = true;
        yield return StartCoroutine(RotateTo(Camera.main.transform.position));

        this.win = win;

        EndGame();
    }

    public void EndGame()
    {
        
        if (win)
        {
            GameManager.Instance.WinGame();
        }
        else
        {
            GameManager.Instance.LoseGame();
        }
    }

    void Move()
    {
        if (Input.GetMouseButtonDown(0) && !interacting && !endGame && !GameManager.Instance.pause)
        {
           

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                Interactuable inter = hit.collider.gameObject.GetComponent<Interactuable>();

                if(inter == null)
                {
                    if (hit.transform.parent != null)
                    {
                        inter = hit.transform.parent.GetComponent<Interactuable>();
                    }
                }
                anim.SetFloat("IdlePose", 0);
                if (inter != null)
                {
                    click = hit.point;
                    actualTask = inter;
                    if (actionInProcess != null)
                    {
                        MarkObjectFloor.transform.rotation = Quaternion.Euler(0, 0, 0);
                        MarkObjectFloor.transform.localScale = Vector3.one;
                        MarkObjectFloor.SetActive(false);

                        MarkObject.transform.SetParent(null);
                        MarkObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                        MarkObject.transform.localScale = Vector3.one;
                        MarkObject.SetActive(false);


                        StopAllCoroutines();
                        actionInProcess = null;
                    }
                    if (inter.PreAction(this))
                    {

                        if (SetDestination(hit.point))
                        {
                            
                            actionInProcess = StartCoroutine(GoToPoint());
                        }

                    }
                    else
                    {
                        anim.SetTrigger(Constantes.ANIMATION_PLAYER_CLICINVALID);
                    }
                }
                else if (!interacting && hit.collider.CompareTag("Player")) 
                {
                    if (agent.velocity.magnitude < 0.1f)
                    {
                        anim.SetTrigger(Constantes.ANIMATION_PLAYER_CLICK);
                    }
                    else
                    {
                        anim.SetTrigger(Constantes.ANIMATION_PLAYER_CLICKMOV);
                    }
                }
                else
                {
                    anim.SetTrigger(Constantes.ANIMATION_PLAYER_CLICINVALID);
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

    private void OnAnimatorIK(int layerIndex)
    {
        //if(IK1 != null)
        //{
        //    anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        //    anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        //    anim.SetIKPosition(AvatarIKGoal.RightHand, IK1.position);
        //}

        //if (IK2 != null)
        //{
        //    anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        //    anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        //    anim.SetIKPosition(AvatarIKGoal.LeftHand, IK2.position);
        //}

        if(ikvalue > 0)
        {
            anim.anim.SetLookAtPosition(Camera.main.transform.position);
            anim.anim.SetLookAtWeight(ikvalue);
        }
    }

    public void EndInteraction()
    {
        interacting = false;
    }

    public void ChangeIdle()
    {
        float x = r.Next(0,3);
        anim.SetFloat("IdlePose", x);
        
    }
}
