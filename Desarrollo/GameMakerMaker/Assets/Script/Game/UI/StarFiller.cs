using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarFiller : MonoBehaviour {

    public Sprite FullStar;
    private Image[] stars;
    public int finalScore;

    public TextMeshProUGUI propsBText, propsMText, propsDText, hornosText, papeleraText, tuberiasText;
    public GameObject[] texts;

    private WaitForSeconds wait;

	// Use this for initialization
	void Start () {
        wait = new WaitForSeconds(0.25f);
        stars = GetComponentsInChildren<Image>();
        foreach(var e in stars)
        {
            e.gameObject.SetActive(false);
        }
        foreach (var e in texts)
        {
            e.gameObject.SetActive(false);
        }
    }
	
    public IEnumerator FillStars(float points,float maxPoints)
    {
        finalScore = 0;
        maxPoints = maxPoints == 0 ? points : maxPoints;
        float completed = (points) / maxPoints;
        finalScore = Mathf.FloorToInt(completed * 3);
        finalScore = Mathf.Max(1, finalScore);
        finalScore = Mathf.Min(finalScore, 3);
        yield return wait;
        if (ScoreController.Instance.propComplejo > 0)
        {
            propsDText.text = ScoreController.Instance.propComplejo.ToString();
            texts[0].SetActive(true);
        }
        yield return wait;
        if (ScoreController.Instance.propNormal > 0)
        {
            propsMText.text = ScoreController.Instance.propNormal.ToString();
            texts[1].SetActive(true);
        }
        yield return wait;
        if (ScoreController.Instance.propBasico > 0)
        {
            propsBText.text = ScoreController.Instance.propBasico.ToString();
            texts[2].SetActive(true);
        }
        yield return wait;
        if (ScoreController.Instance.hornos > 0)
        {
            hornosText.text = ScoreController.Instance.hornos.ToString();
            texts[3].SetActive(true);
        }
        yield return wait;
        if (ScoreController.Instance.entregas > 0)
        {
            tuberiasText.text = ScoreController.Instance.entregas.ToString();
            texts[4].SetActive(true);
        }
        yield return wait;
        if (ScoreController.Instance.papelera > 0)
        {
            papeleraText.text = ScoreController.Instance.papelera.ToString();
            texts[5].SetActive(true);
        }



        
        yield return wait;
        for (int i = 0; i < 3; i++)
        {
            Image star = stars[i];
            star.gameObject.SetActive(true);
            if(i<finalScore)
            {
                
                
                star.sprite = FullStar;
                star.fillAmount = 0;
                while(star.fillAmount < 1)
                {
                    star.fillAmount += 0.05f;
                    yield return null;
                }

                yield return wait;
            }

        }


        GameObject button = GameObject.FindGameObjectWithTag(Constantes.TAG_WIN_BUTTONS);
        foreach(var but in button.GetComponentsInChildren<Button>())
        {
            
            but.interactable = true;
            if(but.name == Constantes.NAME_NEXT && GameManager.Instance.loadedLevel.levelID > 7)
            {
                but.interactable = false;
            }
        }
    }
}
