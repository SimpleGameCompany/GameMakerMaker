using Polyglot;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour {

    public GameObject UI;
    public GameObject Load;
	public void ChangeScene(string scene)
    {
        //UI.SetActive(false);
        //Load.SetActive(true);
        StartCoroutine(SceneLoad(scene));
    }


    [DllImport("__Internal")]
    private static extern void GoCredits();
    [DllImport("__Internal")]
    private static extern void SetSpanish();
    [DllImport("__Internal")]
    private static extern void SetEnglish();

    IEnumerator SceneLoad(string scene)
    {
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(scene);
        while (!sceneLoad.isDone)
        {               
            yield return null;
        }
    }

    public void SelectSpanish()
    {
        Localization.Instance.SelectLanguage(Language.Spanish);
        SetSpanish();
    }

    public void SelectEnglish()
    {
        
        Localization.Instance.SelectLanguage(Language.English);
        SetEnglish();
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteKey("maxlevel");
        GameManager.Instance.maxlevel = 0;
        PlayerPrefs.DeleteKey(Constantes.PREFERENCES_LEVEL_SCORE);
    }

    public void Credits()
    {
        GoCredits();
    }
}
