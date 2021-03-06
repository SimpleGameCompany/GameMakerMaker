﻿using UnityEngine;
using System.Collections;

public static class Constantes
{
    #region Assets
    public const string LEVEL_TAG = "Level";
    public const string LEVEL_DATA_PATH = "Assets/Resources/Levels/";
    public const string LEVEL_ASSET_PATH = LEVEL_DATA_PATH + "LevelDataObjects/";
    public const string LEVEL_PREFAB_PATH = LEVEL_DATA_PATH + "Prefabs/";
    public const string LEVEL_PREDETERMINATE_PATH = LEVEL_DATA_PATH + "Predeterminate/level.prefab";
    public const string LEVEL_GAME_PATH = "Levels/LevelDataObjects/";
    #endregion
    #region Tags
    public const string TAG_LAYOUT = "Layout";
    public const string TAG_PROP_START = "PropStart";
    public const string TAG_RECIPIES = "QuitRecetas";
    public const string TAG_BELT = "Cinta";
    public const string TAG_PAPER = "Papelera";
    public const string TAG_LIFES = "Lifes";
    public const string TAG_WIN_BUTTONS = "WinButtons";
    public const string TAG_END = "EndGameUI";
    public const string TAG_WIN = "WinMenu";
    public const string TAG_STARS = "Stars";
    public const string TAG_INDICATOR = "Indicator";
    #endregion
    #region Scenes
    public const string SCENE_MENU = "MainMenu";
    public const string SCENE_GAME = "GameScene";
    #endregion

    #region Animations
    public const string ANIMATION_PLAYER_DROP_OBJECT = "DropObject";
    public const string ANIMATION_PLAYER_IDLE_POSE = "IdlePose";
    public const string ANIMATION_PLAYER_SPEED = "Speed";
    public const string ANIMATION_PLAYER_LOSE = "Lose";
    public const string ANIMATION_PLAYER_WIN = "Win";
    public const string ANIMATION_PLAYER_PICK = "Pick";
    public const string ANIMATION_PLAYER_ANGULARSPEED = "AngularSpeed";
    public const string ANIMATION_PLAYER_ROTATE = "Rotate";
    public const string ANIMATION_PLAYER_CLICK = "Click";
    public const string ANIMATION_PLAYER_CLICKMOV = "ClickMov";
    public const string ANIMATION_PLAYER_CLICINVALID = "InvalidClick";

    public const string ANIMATION_OVEN_DROP_OBJECT = "DropObject";
    public const string ANIMATION_OVEN_GET_OBJECT = "GetObject";
    public const string ANIMATION_OVEN_BREAK = "Break";
    public const string ANIMATION_OVEN_FIXED = "Fixed";
    public const string ANIMATION_OVEN_COOK = "Cook";

    public const string ANIMATION_PROP_SPAWN = "Spawn";
    public const string ANIMATION_PROP_DESTROY = "Destroy";
    public const string ANIMATION_PROP_GRAB = "Grabed";
    public const string ANIMATION_PROP_SCALEUP = "ScaleUp";
    public const string ANIMATION_PROP_SCALEDOWN = "ScaleDown";
    public const string ANIMATION_OVEN_COOK_END = "EndCook";

    public const string ANIMATION_TUBE_FIXED = "Fix";


    #endregion

    #region PlayerPreferences
    public const string PREFERENCES_MAX_SCORE = "maxscore";
    public const string PREFERENCES_LEVEL_SCORE = "levelScore";

    public const string TAG_RECIPIES_CONTAINER = "RecetasContainer";

    public const string ANIMATION_OVEN_MARKED = "Highlight";


    




    public const string NAME_NEXT = "Next";

    public const string KEY_OVEN_TAKE = "GAME_TUTORIAL_OVEN_TAKE";

    public const string KEY_OVEN_GOTO = "GAME_TUTORIAL_OVEN_GOTO";

    public const string KEY_OVEN_COOKING = "GAME_TUTORIAL_OVEN_COOKING";

    public const string KEY_TUBE_GOTO = "GAME_TUTORIAL_TUBE_GOTO";




    #endregion

    #region Score
    public const string PROP_BASIC = "prop_Basic";
    public const string PROP_MEDIUM = "prop_Medium";
    public const string PROP_HARD = "prop_Hard";
    public const string TUBE = "Tube";
    public const string OVEN = "Oven";
    public const string PAPER = "Paper";
    #endregion
}
