using Polyglot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityController : MonoBehaviour {
    static int actualLevel = 2;
    public string[] KeyQualityTypes;
    public LocalizedTextMeshProUGUI text;

	void Start () {
       
        QualitySettings.SetQualityLevel(actualLevel);
        
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
