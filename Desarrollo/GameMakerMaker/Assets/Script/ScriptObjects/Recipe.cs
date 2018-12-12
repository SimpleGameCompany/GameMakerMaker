using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewRecipe", menuName = "GameMakerMaker/Recipe", order = 2)]
public class Recipe : ScriptableObject {

    public OvenInstruction[] Tasks;
    public int score;
}
