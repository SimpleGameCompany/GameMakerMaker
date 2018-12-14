using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelLoaderButton : MonoBehaviour
{
    [HideInInspector]
    public Level level; 
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(startScene);
    }

    public void startScene()
    {
        GameManager.Instance.LoadLevel(level);
    }
}
