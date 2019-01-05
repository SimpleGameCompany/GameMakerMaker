using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPageController : MonoBehaviour {

    private GameObject[] PageList;
    [SerializeField]
    private GameObject pagePrefab;
    [SerializeField]
    private GameObject next;
    [SerializeField]
    private GameObject previous;
    public Sprite fillStar;
    public Sprite emptyStar;
    private int page;
    private int maxPage;
    public int ActualPage { get { return page; }
        set {
            
            PageList[page].SetActive(false);
            page = value;
            PageList[page].SetActive(true);
            previous.SetActive(page !=0);
            next.SetActive(page != maxPage-1);
            

        } }
    void OnEnable()
    {
        ActualPage = 0;

        int maxlevel = 0;
        if (PlayerPrefs.HasKey("maxlevel"))
        {
            maxlevel = PlayerPrefs.GetInt("maxlevel");
        }

        List<LevelScore> levelScore = new List<LevelScore>();
        if (PlayerPrefs.HasKey(Constantes.PREFERENCES_LEVEL_SCORE))
        {
            string t = PlayerPrefs.GetString(Constantes.PREFERENCES_LEVEL_SCORE);
            levelScore = JsonConvert.DeserializeObject<List<LevelScore>>(t);
        }

        for (int i = 0; i < maxPage; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                LevelLoaderButton[] b = PageList[i].GetComponentsInChildren<LevelLoaderButton>(true);
                int index = (3 * i) + j;
                if (index < GameManager.Instance.levels.Length)
                {
                    LevelLoaderButton b1 = b[j];
                    Image[] stars = b1.Stars.GetComponentsInChildren<Image>();
                    stars[0].sprite = emptyStar;
                    stars[1].sprite = emptyStar;
                    stars[2].sprite = emptyStar;
                    b1.GetComponent<Button>().interactable = false;
                    if (index <= maxlevel)
                    {
                        b1.GetComponent<Button>().interactable = true;
                    }
                    if (levelScore.Count > 0)
                    {
                        LevelScore l = (from x in levelScore where x.levelID == index select x).FirstOrDefault();
                        if (l != null)
                        {
                            for (int v = 0; v < l.score; v++)
                            {
                                stars[v].sprite = fillStar;
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }


    private void Awake()
    {
        
        

        Level [] levels = Resources.LoadAll<Level>(Constantes.LEVEL_GAME_PATH).OrderBy(x => x.levelID).ToArray();
        GameManager.Instance.levels = levels;

        maxPage = (levels.Length / 4) + 1;
        PageList = new GameObject[maxPage];
        for(int i = 0; i < maxPage; i++)
        {
            PageList[i] = Instantiate(pagePrefab, this.transform);
            PageList[i].SetActive(false);
            for ( int j = 0; j<3; j++)
            {
                LevelLoaderButton[] b = PageList[i].GetComponentsInChildren<LevelLoaderButton>(true);
                int index = (3 * i) + j;
                if (index <levels.Length)
                {
                    LevelLoaderButton b1 = b[j];
                    b1.level = levels[index];
                    b1.gameObject.SetActive(true);
                    
                }
                else
                {
                    break;
                }
            }
        }
    }

    public void nextPage()
    {
        ActualPage++;
    }

    public void previousPage()
    {
        ActualPage--;
    }
}
