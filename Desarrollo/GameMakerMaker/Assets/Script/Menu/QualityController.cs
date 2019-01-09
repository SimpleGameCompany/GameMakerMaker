using Polyglot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityController : MonoBehaviour {
    static int actualLevel = 0;
    public string[] KeyQualityTypes;
    public LocalizedTextMeshProUGUI text;
    public static int pixelcount = 0;
	void Start () {
       
        QualitySettings.SetQualityLevel(actualLevel);
        pixelcount = QualitySettings.pixelLightCount;
        text.Key = KeyQualityTypes[actualLevel];
	}
	
	// Update is called once per frame



    public void NextQuality() {

        actualLevel = ++actualLevel % 4;
        text.Key = KeyQualityTypes[actualLevel];
        QualitySettings.SetQualityLevel(actualLevel);
        pixelcount = QualitySettings.pixelLightCount;
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
        pixelcount = QualitySettings.pixelLightCount;
    }
}
