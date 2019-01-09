using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeController : MonoBehaviour {


    private static LifeController _instance;

    public Sprite FullHearth;
    public Sprite HalfHearth;
    public Sprite EmptyHearth;

    int maxLifes;

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

            for (int i = lifes; i < maxLifes; i++)
            {
                Sprites[i].GetComponent<Image>().sprite = EmptyHearth;
            }

            if (lifes == 0 &&  GameManager.Instance.playing)
            {
                GameManager.Instance.EndGame(false);
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

    public GameObject[] Sprites;



    void Start () {
        maxLifes = Sprites.Length;
        for (int i = 0; i < maxLifes; i++)
        {
            Sprites[i].GetComponent<Image>().sprite = FullHearth;
        }
        Lifes = maxLifes;
	}
}
