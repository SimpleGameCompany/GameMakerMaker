using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Polyglot;
using UnityEngine.Video;

[RequireComponent(typeof(Animator),typeof(RectTransform),typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler), typeof(GraphicRaycaster))]
public class LoadManager : MonoBehaviour
{

    private enum LoadingType
    {
        GameManager,
        Async,
        None
    }

    #region LoadVariables
   

    public static LoadManager Instance;
    // Make sure the loading screen shows for at least 1 second:
    private const float MIN_TIME_TO_SHOW = 1f;
    // The reference to the current loading operation running in the background:
    private GameManager currentLoadingOperation;
    private AsyncOperation currentAsync;
    // A flag to tell whether a scene is being loaded or not:
    private bool isLoading;
    private bool timePassed;
    // The rect transform of the bar fill game object:
    [SerializeField]
    private RectTransform barFillRectTransform;
    // Initialize as the initial local scale of the bar fill game object. Used to cache the Y-value (just in case):
    private Vector3 barFillLocalScale;
    // The text that shows how much has been loaded:
    [SerializeField]
    private TextMeshProUGUI percentLoadedText;
    // The elapsed time since the new scene started loading:
    private float timeElapsed;
    // Set to true to hide the progress bar:
    [SerializeField]
    private bool hideProgressBar;
    // Set to true to hide the percentage text:
    [SerializeField]
    private bool hidePercentageText;
    // The animator of the loading screen:
    private Animator animator;
    // Flag whether the fade out animation was triggered.
    private bool didTriggerFadeOutAnimation;


    private LoadingType type = LoadingType.None;
    #endregion

    private void Awake()
    {
        #region Singleton
        // Singleton logic:
        if (Instance == null)
        {
            Instance = this;
            // Don't destroy the loading screen while switching scenes:
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        Configure();
        Hide();
        #endregion
    }

    #region LoadMethods
    private void Configure()
    {
        //Prepare the videos
       
        // Save the bar fill's initial local scale:
        barFillLocalScale = barFillRectTransform.localScale;
        // Enable/disable the progress bar based on configuration:
        barFillRectTransform.transform.parent.gameObject.SetActive(!hideProgressBar);
        // Enable/disable the percentage text based on configuration:
        percentLoadedText.gameObject.SetActive(!hidePercentageText);
        // Cache the animator:
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (isLoading)
        {
            switch (type)
            {
                case LoadingType.GameManager:
                    if (isLoading)
                    {
                        SetProgress(currentLoadingOperation.progress);
                        // If the loading is complete and the fade out animation has not been triggered yet, trigger it:
                        if (currentLoadingOperation.isDone && !didTriggerFadeOutAnimation)
                        {
                            animator.SetTrigger("Hide");

                            didTriggerFadeOutAnimation = true;
                        }
                        else
                        {
                            timeElapsed += Time.deltaTime;
                            if (timeElapsed >= MIN_TIME_TO_SHOW)
                            {
                                // The loading screen has been showing for the minimum time required.
                                // Allow the loading operation to formally finish:
                                currentAsync.allowSceneActivation = true;
                            }
                        }
                    }
                    break;
                case LoadingType.Async:
                    if (isLoading)
                    {
                        SetProgress(currentAsync.progress);
                        // If the loading is complete and the fade out animation has not been triggered yet, trigger it:
                        if (currentAsync.isDone && !didTriggerFadeOutAnimation)
                        {
                            animator.SetTrigger("Hide");

                            didTriggerFadeOutAnimation = true;
                        }
                        else
                        {
                            timeElapsed += Time.deltaTime;
                            if (timeElapsed >= MIN_TIME_TO_SHOW)
                            {
                                // The loading screen has been showing for the minimum time required.
                                // Allow the loading operation to formally finish:
                                currentAsync.allowSceneActivation = true;
                            }
                        }
                    }
                    break;
                case LoadingType.None:
                    break;
            }            // Get the progress and update the UI. Goes from 0 (start) to 1 (end):
            
        }
    }
    // Updates the UI based on the progress:
    private void SetProgress(float progress)
    {
        // Update the fill's scale based on how far the game has loaded:
        barFillLocalScale.x = progress;
        // Update the rect transform:
        barFillRectTransform.localScale = barFillLocalScale;
        // Set the percent loaded text:
        percentLoadedText.text = Mathf.CeilToInt(progress * 100).ToString() + "%";
    }
    // Call this to show the loading screen.
    // We can determine the loading's progress when needed from the AsyncOperation param:
    public void Show(GameManager loadingOperation, AsyncOperation loadingOperationAsync)
    {
        // Enable the loading screen:
        gameObject.SetActive(true);
        // Store the reference:
        currentLoadingOperation = loadingOperation;
        currentAsync = loadingOperationAsync;
        // Stop the loading operation from finishing, even if it technically did:
        currentAsync.allowSceneActivation = false;
        // Reset the UI:
        SetProgress(0f);
        // Reset the time elapsed:
        timeElapsed = 0f;
        // Play the fade in animation:
        //animator.SetTrigger("Show");
        // Reset the fade out animation flag:


        GetComponentInChildren<RawImage>().texture = VideoPage.instance.getRandomVideo();


        type = LoadingType.GameManager;
        didTriggerFadeOutAnimation = false;
        isLoading = true;
    }

    public void Show (AsyncOperation loadingOperation)
    {
        // Enable the loading screen:
        gameObject.SetActive(true);
        // Store the reference:
        currentAsync = loadingOperation;
        // Stop the loading operation from finishing, even if it technically did:
        currentAsync.allowSceneActivation = false;
        // Reset the UI:
        SetProgress(0f);
        // Reset the time elapsed:
        timeElapsed = 0f;
        // Play the fade in animation:
        //animator.SetTrigger("Show");
        // Reset the fade out animation flag:

        GetComponentInChildren<RawImage>().texture = VideoPage.instance.getRandomVideo();

        didTriggerFadeOutAnimation = false;
        type = LoadingType.Async;
        isLoading = true;
    }
    // Call this to hide it:
    public void Hide()
    {
        // Disable the loading screen:
        if(VideoPage.instance != null)
            VideoPage.instance.StopVideo();
        gameObject.SetActive(false);
        currentLoadingOperation = null;
        currentAsync = null;
        type = LoadingType.None;
        isLoading = false;
    }
    #endregion
}
