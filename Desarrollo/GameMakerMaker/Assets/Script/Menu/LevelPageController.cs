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
       Level [] levels = Resources.LoadAll<Level>(Constantes.LEVEL_GAME_PATH).OrderBy((x)=> { return x.levelID; }).ToArray();
        maxPage = (levels.Length / 3) + 1;
        PageList = new GameObject[maxPage];
        for(int i = 0; i < maxPage; i++)
        {
            PageList[i] = Instantiate(pagePrefab, this.transform);
            for ( int j = 0; j<3; j++)
            {
                LevelLoaderButton[] b = PageList[i].GetComponentsInChildren<LevelLoaderButton>(true);
                if ((3*i)+j <levels.Length)
                {
                    LevelLoaderButton b1 = b[j];
                    b1.level = levels[3 * i + j];
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
