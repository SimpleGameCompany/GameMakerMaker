using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour {

    private static ScoreController _instance;

    [HideInInspector]
    public int hornos, papelera, entregas, propNormal, propBasico, propComplejo;
    [HideInInspector]
    public float hornosScore, papeleraScore, entregasScore, propNormalScore, propBasicoScore, propComplejoScore;

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
    public float Score { get { return score; } }

    public void AddScore(float p,float s, string type)
    {

        if (GameManager.Instance.playing)
        {
            score += p;
            Mathf.Clamp(scoreNumber += s, 0, int.MaxValue);
            textComponent.text = text + ": " + score + "/" + GameManager.Instance.loadedLevel.winCount;

            switch (type)
            {
                case Constantes.PROP_BASIC:
                    propBasico++;
                    propBasicoScore += score;
                    break;
                case Constantes.PROP_MEDIUM:
                    propNormal++;
                    propNormalScore += score;
                    break;
                case Constantes.PROP_HARD:
                    propComplejo++;
                    propComplejoScore += score;
                    break;
                case Constantes.TUBE:
                    entregas++;
                    entregasScore += score;
                    break;
                case Constantes.OVEN:
                    hornos++;
                    hornosScore += score;
                    break;
                case Constantes.PAPER:
                    papelera++;
                    papeleraScore += score;
                    break;
                default:
                    break;
            }

            if (score == GameManager.Instance.loadedLevel.winCount)
            {
                GameManager.Instance.EndGame(true);
            }
        }
    }

    private string text;
    private TextMeshProUGUI textComponent;
    public float scoreNumber;

    void Start () {      
        textComponent = GetComponent<TextMeshProUGUI>();
       
        text = textComponent.text;
        score = 0;
        scoreNumber = 0;
    }


    private void OnDestroy()
    {
        _instance = null;
    }
}
