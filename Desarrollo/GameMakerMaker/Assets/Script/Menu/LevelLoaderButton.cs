﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class LevelLoaderButton : MonoBehaviour
{
   
    public Level level;
    public GameObject Stars;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(startScene);
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        
    }

    public void startScene()
    {
        GameManager.Instance.LoadLevel(level);
    }

    private void OnEnable()
    {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        if(level.levelID == 0)
        {
            text.text = "Tutorial";
        }else
            text.text = text.text + " "+ (level.levelID);
    }
}
