using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour {

	// Use this for initialization
	void Start () {
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
        GameManager.Instance.ResumeGame();
    }

    public void NextLevel() {
        GameManager.Instance.ResumeGame();
        GameManager.Instance.NextLevel();
    }

}
