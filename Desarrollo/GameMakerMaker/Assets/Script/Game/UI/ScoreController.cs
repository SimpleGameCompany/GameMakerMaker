using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour {

    private static ScoreController _instance;

    public static ScoreController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<ScoreController>();
            }
            return _instance;
        }

        private set
        {
            _instance = value;
        }
    }



    private float score;
    public float Score { get { return score; }
        set {
            score = value;
            textComponent.text = text + ": " + score+"/"+GameManager.Instance.loadedLevel.winCount;
            if (score == GameManager.Instance.loadedLevel.winCount)
            {
                GameManager.Instance.EndGame(true);
            }
                              } }

   

    private string text;
    private TextMeshProUGUI textComponent;
    public float scoreNumber;

    void Start () {      
        textComponent = GetComponent<TextMeshProUGUI>();
       
        text = textComponent.text;
        Score = 0;
        scoreNumber = 0;
    }


    private void OnDestroy()
    {
        _instance = null;
    }
}
