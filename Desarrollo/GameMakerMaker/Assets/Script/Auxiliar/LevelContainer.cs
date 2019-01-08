using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class LevelContainer 
{
    public LevelContainer()
    {
        Scores = new List<LevelScore>();
    }
    public List<LevelScore> Scores;

}
