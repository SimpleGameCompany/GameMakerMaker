using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEditorWindow : EditorWindow {



    static Level l;
    static float velocity;
    static float ratio;
    static int winCount;
    static string levelName = "level";
    static bool Pick = false;
    [SerializeField]
    PropBehaviour[] Props;
    static PropBehaviour[] staticProps;
    [MenuItem("GameMakerMaker/LevelEditor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        if (SceneManager.GetActiveScene().name == "LevelEditor")
        {
            LevelEditorWindow window = (LevelEditorWindow)EditorWindow.GetWindow(typeof(LevelEditorWindow));
            
            window.Show();
        }
    }



    private void OnGUI()
    {
        Props = staticProps;
        GUILayout.Label("Level Editor", EditorStyles.boldLabel);
        ScriptableObject e = this;
        SerializedObject o = new SerializedObject(e);  
        levelName  = EditorGUILayout.TextField("Level Name",levelName);
        EditorGUILayout.PropertyField(o.FindProperty("Props"), true);
        velocity = EditorGUILayout.FloatField("Velocity", velocity);
        ratio = EditorGUILayout.FloatField("Ratio", ratio);
        winCount = EditorGUILayout.IntField("Props to Win", winCount);
        o.ApplyModifiedProperties();
        staticProps = Props;
        



        if (GUILayout.Button("Save Level"))
        {
            Save();
        }

        if(GUILayout.Button("Load Level"))
        {
            Load();
        }
        string commandName = Event.current.commandName;
        if (commandName == "ObjectSelectorClosed")
        {
            OnPick();
        }

    }



    private void OnPick()
    {
        if (Pick)
        {
            l = (Level)EditorGUIUtility.GetObjectPickerObject();
            if (l != null)
            {
                DestroyImmediate(GameObject.FindGameObjectWithTag(Constantes.LEVEL_TAG));
                levelName = l.LevelName;
                Props = l.Props;
                PrefabUtility.InstantiatePrefab(l.Scenario);
                Pick = false;
                Repaint();
            }
        }
    }

    private void Load()
    {
        int controlID = EditorGUIUtility.GetControlID(FocusType.Passive);
        Pick = true;
        EditorGUIUtility.ShowObjectPicker<Level>(null, false, "", controlID);
    }


    private void Save()
    {
        Level level = ScriptableObject.CreateInstance<Level>();
        
        level.LevelName = levelName;
        level.Scenario = GameObject.FindGameObjectWithTag(Constantes.LEVEL_TAG);
        level.Scenario = PrefabUtility.CreatePrefab(Constantes.LEVEL_PREFAB_PATH+ levelName+".prefab", level.Scenario);
        level.velocity = velocity;
        level.winCount = winCount;
        level.ratio = ratio;
        string path = Constantes.LEVEL_ASSET_PATH +levelName+ ".asset";
        AssetDatabase.CreateAsset(level, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = level;
    }

    





}
