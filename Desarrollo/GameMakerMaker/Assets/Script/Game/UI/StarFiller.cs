using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarFiller : MonoBehaviour {

    public Sprite FullStar;
    private Image[] stars;
    public int finalScore;
	// Use this for initialization
	void Start () {
        stars = GetComponentsInChildren<Image>();
        foreach(var e in stars)
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
        for(int i = 0; i < 3; i++)
        {
            Image star = stars[i];
            star.gameObject.SetActive(true);
            if(completed > 0)
            {
                
                completed -= 1/3f;
                star.sprite = FullStar;
                star.fillAmount = 0;
                while(star.fillAmount < 1)
                {
                    star.fillAmount += 0.05f;
                    yield return null;
                }

                yield return new WaitForSeconds(0.5f);
            }

        }
        GameObject button = GameObject.FindGameObjectWithTag(Constantes.TAG_WIN_BUTTONS);
        foreach(var but in button.GetComponentsInChildren<Button>())
        {
            
            but.interactable = true;
            if(but.name == Constantes.NAME_NEXT && GameManager.Instance.loadedLevel.levelID >= 7)
            {
                but.interactable = false;
            }
        }
    }
}
