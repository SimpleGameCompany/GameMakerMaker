using Polyglot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityController : MonoBehaviour {
    int actualLevel;
    public string[] KeyQualityTypes;
    public LocalizedTextMeshProUGUI text;
	void Start () {
        actualLevel = 2;
        QualitySettings.SetQualityLevel(2);
        text.Key = KeyQualityTypes[actualLevel];
	}
	
	// Update is called once per frame



    public void NextQuality() {

        actualLevel = ++actualLevel % 4;
        text.Key = KeyQualityTypes[actualLevel];
        QualitySettings.SetQualityLevel(actualLevel);
    }

    public void BackQuality()
    {
        actualLevel--;
        if(actualLevel == -1)
        {
            actualLevel = 3;
        }
        text.Key = KeyQualityTypes[actualLevel];
        QualitySettings.SetQualityLevel(actualLevel);
    }
}
