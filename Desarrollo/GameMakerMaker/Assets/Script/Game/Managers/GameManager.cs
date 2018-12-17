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
    private Vector3 PositionStart;
    private Button Recipies;
    private GameObject UI;
    // Use this for initialization
    void Start()
    {
        totalProps = new List<GameObject>();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void StoreProp(GameObject prop)
    {
        prop.SetActive(false);
        prop.GetComponent<PropBehaviour>().Restart();
        prop.transform.SetParent(gameObject.transform);
        totalProps.Add(prop);
    }

    public void LoadLevel(Level l)
    {
        loadedLevel = l;
        SceneManager.LoadScene("GameScene");
    }

     public void StartGame()
    {
        GameObject scenario = Instantiate(loadedLevel.Scenario);
        scenario.GetComponentInChildren<NavMeshSurface>().BuildNavMesh();
        GameObject[] props = (from x in loadedLevel.Props select x.gameObject).ToArray();
        for(int i = 0; i < props.Length; i++)
        {
            int number = loadedLevel.MaxProps[i];
            while(number > 0)
            {
                GameObject o = Instantiate(props[i]);
                o.SetActive(false);
                totalProps.Add(o);
                number--;
            }
        }
        PositionStart = GameObject.FindGameObjectWithTag(Constantes.TAG_PROP_START).transform.position;
        BeltBehaviour[] n = FindObjectsOfType<BeltBehaviour>();
        foreach(var e in n) { e.speed = loadedLevel.velocity; }
        Button recetas = GameObject.FindGameObjectWithTag(Constantes.TAG_RECIPIES).GetComponent<Button>();

        recetas.onClick.AddListener(delegate { PlayGame(); });

        Recipies = recetas;
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
            prop.SetActive(true);
            prop.transform.position = PositionStart;
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
}
