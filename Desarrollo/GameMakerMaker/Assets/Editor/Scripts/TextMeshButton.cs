using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TextMeshButton: Editor {


    [MenuItem("GameObject/UI/TextMeshButton")]
    public static void Action()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath("Assets/Editor/Prefabs/TextMeshButton.prefab",typeof(GameObject)) as GameObject;
        prefab = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        prefab.transform.SetParent(Selection.activeTransform);
        prefab.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);

    }

}
