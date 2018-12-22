using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour {


    private static LifeController _instance;


    private int lifes = 3;

    public int Lifes
    {
        get
        {
            return lifes;
        }

        set
        {
            lifes = value;
            if(lifes == 0)
            {
                GameManager.Instance.EndGame();
            }
            else
            {

                for (int i = 0; i<3; i++)
                {                                  
                    Sprites[i].SetActive(i < lifes);                                         
                }


            }

        }
    }

    public static LifeController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<LifeController>();
            }
            return _instance;
        }

        private set
        {
            _instance = value;
        }
    }

    private GameObject[] Sprites;



    void Start () {
        Sprites = GameObject.FindGameObjectsWithTag(Constantes.TAG_LIFES);
        Lifes = 3;
	}
}
