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
        SceneManager.LoadScene(Constantes.SCENE_MENU);
    }

    public void ReStart()
    {
        GameManager.Instance.ResumeGame();
        GameManager.Instance.ReStart();
        SceneManager.LoadScene(Constantes.SCENE_GAME);
    }

    public void StoreAndBack()
    {
        GameManager.Instance.ResumeGame();
        BackToMenu();
    }

    public void Continue()
    {
        GameManager.Instance.ResumeGame();
    }

}
