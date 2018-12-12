using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    //TODO Posiblemente vendria bien tener la lista de maximos elementos en pantalla, generarlos 
    List<GameObject> totalProps; 
    // Use this for initialization
    void Start()
    {
        totalProps = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void StoreProp(GameObject prop)
    {
        prop.SetActive(false);
        prop.transform.SetParent(gameObject.transform);
        totalProps.Add(prop);
    }
}
