using Polyglot;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour {

    public GameObject UI;
    public GameObject Load;
    public Button Spanish, English;
	public void ChangeScene(string scene)
    {
        //UI.SetActive(false);
        //Load.SetActive(true);
        StartCoroutine(SceneLoad(scene));
    }

#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void GoCredits();
    [DllImport("__Internal")]
    private static extern void SetSpanish();
    [DllImport("__Internal")]
    private static extern void SetEnglish();
    [DllImport("__Internal")]
    private static extern void Resize();
#endif

#if UNITY_ANDROID
    private static void GoCredits() { }
    
    private static  void SetSpanish() { }
    
    private static void SetEnglish()    { }
   
    private static  void Resize() { }
#endif
    public void Awake()
    {
        try
        {
            Resize();
        }
        catch
        {
            Debug.Log("Funcion de javaScript");
        }
        if (Localization.Instance.SelectedLanguage == Language.Spanish)
        {
            SelectSpanish();
        }
        else if(Localization.Instance.SelectedLanguage == Language.English)
        {
            SelectEnglish();
        }
    }

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
        Spanish.interactable = false;
        English.interactable = true;
        try
        {
            SetSpanish();
        }
        catch
        {
            Debug.Log("Funcion de javaScript");
        }
    }

    public void SelectEnglish()
    {
        
        Localization.Instance.SelectLanguage(Language.English);
        Spanish.interactable = true;
        English.interactable = false;
        try
        {
            SetEnglish();
        }
        catch
        {
            Debug.Log("Funcion de javaScript");
        }
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteKey("maxlevel");
        GameManager.Instance.maxlevel = 1;
        PlayerPrefs.DeleteKey(Constantes.PREFERENCES_LEVEL_SCORE);
    }

    public void Credits()
    {
        GoCredits();
    }
}
