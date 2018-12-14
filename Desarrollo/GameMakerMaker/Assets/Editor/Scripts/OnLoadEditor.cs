using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class OnLoadEditor: Editor  {

	
    static OnLoadEditor()
    {
        EditorSceneManager.sceneOpened += OnSceneSaved;
    }

    static void OnSceneSaved(UnityEngine.SceneManagement.Scene scene, UnityEditor.SceneManagement.OpenSceneMode mode)
    {
        Debug.Log(scene.name);
        if(scene.name == "LevelEditor")
        {
            
            DestroyImmediate(GameObject.FindGameObjectWithTag(Constantes.LEVEL_TAG));
            GameObject b =(GameObject) AssetDatabase.LoadAssetAtPath("Assets/Resources/Levels/Predeterminate/level.prefab", typeof(GameObject));
            PrefabUtility.InstantiatePrefab(b);
        }
    }
}
