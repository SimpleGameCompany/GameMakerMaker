using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectCreator : Editor {
   static bool Pick = false;
    [MenuItem("GameObject/GameMakerMaker/Ovens")]
    public static void Action()
    {
        int controlID = EditorGUIUtility.GetControlID(FocusType.Passive);
        Pick = true;
        EditorGUIUtility.ShowObjectPicker<GameObject>(null, false, "", controlID);
        
    }

    private void Update()
    {
        Debug.Log("Hello");
        string commandName = Event.current.commandName;
        if (commandName == "ObjectSelectorClosed" && Pick)
        {
            Pick = false;
            GameObject l = (GameObject)EditorGUIUtility.GetObjectPickerObject();
            Instantiate(l, Selection.activeTransform);
        }

    }

    private void OnEnable()
    {
        EditorApplication.update += Update;
    }
}
