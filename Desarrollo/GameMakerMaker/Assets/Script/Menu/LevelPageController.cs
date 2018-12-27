using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	void Start () {
        ActualPage = 0;
	}

    private void Awake()
    {
        int maxlevel = 0;
        if (PlayerPrefs.HasKey("maxlevel"))
        {
            maxlevel = PlayerPrefs.GetInt("maxlevel");
        }

        Level [] levels = Resources.LoadAll<Level>(Constantes.LEVEL_GAME_PATH).OrderBy(x => x.levelID).ToArray();
        maxPage = (levels.Length / 3) + 1;
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
                    if (index<=maxlevel)
                    {
                        b1.GetComponent<Button>().interactable = true;
                    }
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
