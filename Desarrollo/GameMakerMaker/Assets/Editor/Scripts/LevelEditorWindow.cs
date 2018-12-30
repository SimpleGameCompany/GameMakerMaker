using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEditorWindow : EditorWindow {


    Vector2 scrollPosition = Vector2.zero;
    static Level l;
    static int ID;
    static float velocity;
    static float ratio;
    static int winCount;
    static string levelName = "level";
    static bool Pick = false;
    static int[] MaxProps;
    static Color AmbientColor;
   
    [SerializeField]
    PropBehaviour[] Props;
    [SerializeField]
    int[] countProps;
    static PropBehaviour[] staticProps;

    static LevelGrid g;

    [MenuItem("GameMakerMaker/LevelEditor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        if (SceneManager.GetActiveScene().name == "LevelEditor")
        {
            LevelEditorWindow window = (LevelEditorWindow)EditorWindow.GetWindow(typeof(LevelEditorWindow));
            window.name = "Level Editor";
            window.Show();
        }
    }

    private void OnEnable()
    {
        g = new LevelGrid();
    }


    private void OnGUI()
    {
        Props = staticProps;
        countProps = MaxProps;
        GUILayout.Label("Level Editor", EditorStyles.boldLabel);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true);
        ScriptableObject e = this;
        SerializedObject o = new SerializedObject(e);
        ID = EditorGUILayout.IntField("ID", ID);
        levelName  = EditorGUILayout.TextField("Level Name",levelName);
        EditorGUILayout.PropertyField(o.FindProperty("Props"), true);
        EditorGUILayout.PropertyField(o.FindProperty("countProps"), true);
        velocity = EditorGUILayout.FloatField("Velocity", velocity);
        ratio = EditorGUILayout.FloatField("Ratio", ratio);

        winCount = EditorGUILayout.IntField("Props to Win", winCount);
        GUIContent n = new GUIContent("AmbienColor");
        AmbientColor =  EditorGUILayout.ColorField(n, AmbientColor,true,false,true);
        RenderSettings.ambientLight = AmbientColor;
        o.ApplyModifiedProperties();
        staticProps = Props;
        MaxProps = countProps;

        g.OnGUI();

        if (GUILayout.Button("Save Level"))
        {
            Save();
        }

        if(GUILayout.Button("Load Level"))
        {
            Load();
        }

        

        GUILayout.EndScrollView();
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
                staticProps = l.Props;
                ID = l.levelID;
                velocity = l.velocity;
                winCount = l.winCount;
                ratio = l.ratio;
                MaxProps = l.MaxProps;
                AmbientColor = l.ambientColor;
                PrefabUtility.InstantiatePrefab(l.Scenario);
                Pick = false;
                g.Load(l);
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
        level.levelID = ID;
        level.Props = Props;
        level.MaxProps = MaxProps;
        level.ambientColor = AmbientColor;
        level = g.Save(level);
        
        string path = Constantes.LEVEL_ASSET_PATH +levelName+ ".asset";
        AssetDatabase.CreateAsset(level, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = level;
    }

    





}
