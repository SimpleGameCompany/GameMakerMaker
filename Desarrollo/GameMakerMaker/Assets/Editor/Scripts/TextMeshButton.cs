using Polyglot;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TextMeshButton: Editor {


    [MenuItem("GameObject/UI/TextMeshButton")]
    public static void Action()
    {

        GameObject prefab = new GameObject();
        prefab.name = "Button";
        GameObject Text = new GameObject();
        Text.AddComponent<TextMeshProUGUI>();
        Text.name = "Text";
        Text.GetComponent<TextMeshProUGUI>().text = "Button";
        Text.transform.SetParent(prefab.transform);
        Text.GetComponent<TextMeshProUGUI>().color = Color.black;
        Text.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        Text.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 100);
        Text.AddComponent<LocalizedTextMeshProUGUI>();
        prefab.AddComponent<Button>();
        prefab.AddComponent<RectTransform>();
        prefab.GetComponent<Button>().targetGraphic = prefab.AddComponent<Image>();
        prefab.GetComponent<Image>().type = Image.Type.Sliced;
        prefab.GetComponent<Button>().image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        prefab.transform.SetParent(Selection.activeTransform);
        prefab.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 100);
        prefab.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        prefab.GetComponent<Transform>().position = new Vector3(0, 0, 0);
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = prefab;

    }

}
