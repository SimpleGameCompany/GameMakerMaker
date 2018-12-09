using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel",menuName = "GameMakerMaker/Level",order =1)]
public class Level : ScriptableObject  {
    public string LevelName;
    public GameObject Scenario;
    public OvenBehaviour[] OvenList;
    public float velocity;
    public float ratio;
    public GameObject[] Props;
    public int winCount;
}
