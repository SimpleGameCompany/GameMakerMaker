using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if(_instance == null)
                {
                    
                    GameObject e = new GameObject();
                    e.name = "GameManager";
                    _instance =  e.AddComponent<GameManager>();
                }
            }
            
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }


    //TODO Posiblemente vendria bien tener la lista de maximos elementos en pantalla, generarlos 
    List<GameObject> totalProps;

    [HideInInspector]
    public Level loadedLevel;
    private bool playing;
    internal Vector3 PositionStart;
    internal Button Recipies;
    private GameObject UI;
    private WaitForEndOfFrame frame;
    public Vector3 EndPosition;
    internal GameObject EndGameUI;
    internal PlayerController player;

    public float progress = 0;
    public bool isDone = false;
    // Use this for initialization
    void Start()
    {
        totalProps = new List<GameObject>();
        DontDestroyOnLoad(gameObject);
        frame = new WaitForEndOfFrame();
    }

    public void StoreProp(GameObject prop)
    {
        prop.SetActive(false);
        prop.GetComponent<PropBehaviour>().Restart();
        prop.transform.SetParent(null);
        totalProps.Add(prop);
    }

    public void LoadLevel(Level l)
    {
        loadedLevel = l;
        AsyncOperation load = SceneManager.LoadSceneAsync(Constantes.SCENE_GAME);
        ClearLoad();
        LoadManager.Instance.Show(this);
    }

    public void StartLevelFromCourutine()
    {
        StartCoroutine(StartGame());
        
    }

     public IEnumerator StartGame()
    {
        EndGameUI = GameObject.FindGameObjectWithTag(Constantes.TAG_END);
        EndGameUI.SetActive(false);
        GameObject scenario = Instantiate(loadedLevel.Scenario);
        progress = 0.4f;
        yield return StartCoroutine(CreateNavMesh(scenario));
        progress = 0.6f;
        yield return StartCoroutine(CreateProps());
        progress = 0.8f;
        PositionStart = GameObject.FindGameObjectWithTag(Constantes.TAG_PROP_START).transform.position;
        GameObject papelera = GameObject.FindGameObjectWithTag(Constantes.TAG_PAPER);
        EndPosition = papelera.transform.position;


        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = loadedLevel.ambientColor;

        Button recetas = GameObject.FindGameObjectWithTag(Constantes.TAG_RECIPIES).GetComponent<Button>();
        recetas.onClick.AddListener(delegate { PlayGame(); });
        Recipies = recetas;

        player = FindObjectOfType<PlayerController>();
        progress = 1f;
        isDone = true;
    }


    internal IEnumerator CreateNavMesh(GameObject scenario) {
        GameObject Belt = GameObject.FindGameObjectWithTag(Constantes.TAG_BELT);


        BeltBehaviour[] Cintas = FindObjectsOfType<BeltBehaviour>();


        foreach (var e in Cintas)
        {
            e.transform.SetParent(Belt.transform);
            yield return frame;
        }

        NavMeshSurface[] surfaces = scenario.GetComponentsInChildren<NavMeshSurface>();
        foreach (var i in surfaces)
        {
            i.BuildNavMesh();
            yield return frame;
        }
    }

    internal IEnumerator CreateProps()
    {
        GameObject[] props = (from x in loadedLevel.Props select x.gameObject).ToArray();
        
        for (int i = 0; i < props.Length; i++)
        {
            int number = loadedLevel.MaxProps[i];
            float p = 0.1f / number;



            while (number > 0)
            {
                GameObject o = Instantiate(props[i]);
                o.SetActive(false);
                totalProps.Add(o);
                NavMeshAgent agent = o.GetComponent<NavMeshAgent>();
                agent.enabled = false;

                agent.angularSpeed = 0;
                agent.speed = loadedLevel.velocity;
                number--;
                progress += p;
                yield return frame;
            }
        }
    }

    IEnumerator GenerateProp(WaitForSeconds waiter)
    {
        System.Random r = new System.Random();
        WaitUntil until = new WaitUntil(() => { return totalProps.Count > 0; });
        while (playing)
        {
            yield return until;
            int i = r.Next(0, totalProps.Count);
            GameObject prop = totalProps[i];
            totalProps.RemoveAt(i);
            //prop.transform.rotation = Quaternion.Euler(0, 0, 0);
            prop.SetActive(true);
            prop.GetComponent<PropBehaviour>().grab = true;
            prop.GetComponentInChildren<Collider>().enabled = true;
            prop.GetComponent<Animator>().SetBool(Constantes.ANIMATION_PROP_GRAB,false);
            prop.transform.position = PositionStart;
            NavMeshAgent agent = prop.GetComponent<NavMeshAgent>();
            agent.enabled = true;
            //agent.SetDestination(EndPosition);
            yield return waiter;
            
        }
    }


    public void PlayGame()
    {
        Recipies.transform.parent.parent.gameObject.SetActive(false);
        WaitForSeconds seconds = new WaitForSeconds(loadedLevel.ratio);
        playing = true;
        StartCoroutine(GenerateProp(seconds));
    }


    public void PauseGame(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    //TODO
    public void LoseGame()
    {
        StopAllCoroutines();

        player.StartCoroutine(player.EndAnim(true));

    }

    public void EndGame()
    {


        PauseGame(0);
        EndGameUI.SetActive(true);

    }

    public void ReStart()
    {
        Clear();
        AsyncOperation load = SceneManager.LoadSceneAsync(Constantes.SCENE_GAME);
        LoadManager.Instance.Show(this);
    }

    public void Clear()
    {
        StopAllCoroutines();
        totalProps.Clear();
        ClearLoad();
    }


    private void ClearLoad()
    {
        isDone = false;
        progress = 0;
    }
}
