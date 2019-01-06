using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour {

    public GameObject PageRecetas;
	void Start () {
        GameManager.Instance.RecetasPage = PageRecetas;
        GameManager.Instance.StartLevelFromCourutine();
	}
	

    public void BackToMenu()
    {
        GameManager.Instance.ResumeGame();
        GameManager.Instance.Clear();
        AsyncOperation load = SceneManager.LoadSceneAsync(Constantes.SCENE_MENU);
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
}
