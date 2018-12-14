using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel",menuName = "GameMakerMaker/Level",order =1)]
public class Level : ScriptableObject  {
    public int levelID;
    public string LevelName;
    public GameObject Scenario;
    public float velocity;
    public float ratio;
    public PropBehaviour[] Props;
    public int[] MaxProps;
    public int winCount;
    
}
