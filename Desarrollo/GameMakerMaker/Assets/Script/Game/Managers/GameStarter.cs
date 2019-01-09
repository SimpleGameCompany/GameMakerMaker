using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour {
    public static GameStarter instance;
    public GameObject PageRecetas;
    public GameObject[] NextPageButton;
    public GameObject[] BackPageButton;
    void Start () {
        instance = this;
        if(GameManager.Instance.loadedLevel.levelID == 0)
        {
            QualitySettings.pixelLightCount = 4;
        }
        GameManager.Instance.RecetasPage = PageRecetas;
        GameManager.Instance.StartLevelFromCourutine();
	}
	

    public void BackToMenu()
    {
        QualitySettings.pixelLightCount = QualityController.pixelcount;
        GameManager.Instance.ResumeGame();
        GameManager.Instance.Clear();
        AsyncOperation load = SceneManager.LoadSceneAsync(Constantes.SCENE_MENU);
        MusicController.Instance.PlaySong("Menu_Music");
        LoadManager.Instance.Show(load);
    }

    public void ReStart()
    {
        GameManager.Instance.ResumeGame();
        GameManager.Instance.ReStart();
    }

    public void Continue()
    {
        MusicController.Instance.UnMuteInGame();
        GameManager.Instance.ResumeGame();
    }

    public void NextLevel() {
        GameManager.Instance.ResumeGame();
        GameManager.Instance.NextLevel();
    }
    public void PauseMenu()
    {
        MusicController.Instance.MuteOtherSounds();
        GameManager.Instance.PauseGame(0);
    }


    public void NextPageInit()
    {
        BookBehaviour.instance.InitPage++;
        if(BookBehaviour.instance.Page == BookBehaviour.instance.pages.Length-1)
        {
            NextPageButton[1].SetActive(false);
        }
        if(BookBehaviour.instance.Page > 0)
        {
            BackPageButton[1].SetActive(true);
        }

    }

    public void backPageInit()
    {
        BookBehaviour.instance.InitPage--;
        if (BookBehaviour.instance.Page < BookBehaviour.instance.pages.Length-1)
        {
            NextPageButton[1].SetActive(true);
        }
        if (BookBehaviour.instance.Page == 0)
        {
            BackPageButton[1].SetActive(false);
        }
    }

    public void NextPage()
    {
        BookBehaviour.instance.Page++;
        if (BookBehaviour.instance.Page == BookBehaviour.instance.pages.Length-1)
        {
            NextPageButton[0].SetActive(false);
        }
        if (BookBehaviour.instance.Page > 0)
        {
            BackPageButton[0].SetActive(true);
        }
        
    }

    public void backPage()
    {

        BookBehaviour.instance.Page--;
        if (BookBehaviour.instance.Page < BookBehaviour.instance.pages.Length-1)
        {
            NextPageButton[0].SetActive(true);
        }
        if (BookBehaviour.instance.Page == 0)
        {
            BackPageButton[0].SetActive(false);
        }

    }

    public void ActivaArrows()
    {
        if(BookBehaviour.instance.pages.Length < 2)
        {
            for (int i = 0; i < 2; i++)
            {
                NextPageButton[i].SetActive(false);
            }

        }
        for (int i = 0; i < 2; i++)
        {
            BackPageButton[i].SetActive(false);
        }
    }
}
