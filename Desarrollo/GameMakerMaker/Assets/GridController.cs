using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridController : EditorWindow
{

    static int x;
    static int y;

    static int oldX;
    static int oldY;

    static Material levelFloor = null;
    static Editor levelFloorEditor = null;

    static List<List<GameObject>> level = new List<List<GameObject>>();
    static List<List<Editor>> levelEditors = new List<List<Editor>>();

    [MenuItem("GameMakerMaker/GridController")]
    static void Init()
    {
        GridController window = (GridController)EditorWindow.GetWindow(typeof(GridController));
        window.Show();
    }


    static void ShowWindow()
    {
        GetWindow<GridController>("LevelEditor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Dimensiones", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        x = EditorGUILayout.IntField(x, GUILayout.ExpandWidth(false));
        y = EditorGUILayout.IntField(y, GUILayout.ExpandWidth(false));
        GUILayout.EndHorizontal();

        GUILayout.Label("Suelo", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical(GUILayout.ExpandWidth(false), GUILayout.MaxWidth(320f));
        levelFloor = (Material)EditorGUILayout.ObjectField(levelFloor, typeof(Material), false, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(160f));
        if (levelFloor != null)
        {
            if (levelFloorEditor == null)
                levelFloorEditor = Editor.CreateEditor(levelFloor);

            levelFloorEditor.OnPreviewGUI(GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(false), GUILayout.MaxHeight(100f), GUILayout.MaxWidth(130f)), null);
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        ChangeGrid();


        GUILayout.Label("Level", EditorStyles.boldLabel);
        for (int j = 0; j < y; j++)
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false), GUILayout.MaxWidth(320f));
            for (int i = 0; i < x; i++)
            {
                GUILayout.BeginVertical(GUILayout.ExpandWidth(false), GUILayout.MaxWidth(320f));
                level[i][j] = (GameObject)EditorGUILayout.ObjectField(level[i][j], typeof(GameObject), false, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(160f));

                if (level[i][j] != null)
                {
                    
                    if (levelEditors[i][j] == null)
                        levelEditors[i][j] = Editor.CreateEditor(level[i][j]);

                    levelEditors[i][j].OnPreviewGUI(GUILayoutUtility.GetRect(0,0,GUILayout.ExpandWidth(false), GUILayout.MaxHeight(100f), GUILayout.MaxWidth(130f)),null);
                }
                GUILayout.EndVertical();

            }
            GUILayout.EndHorizontal();
        }
    }

    void ChangeGrid()
    {
        if (x > oldX)
        {
            for (int i = oldX; i < x; i++)
            {

                level.Add(new List<GameObject>());
                levelEditors.Add(new List<Editor>());
                for (int j = 0; j < y; j++)
                {
                    level[i].Add(null);
                    levelEditors[i].Add(null);
                }
            }
            oldX = x;
        }
        else if (x < oldX)
        {
            for (int i = oldX; i > x; i--)
            {
                for (int j = 0; j < y; j++)
                {
                    level[i - 1].RemoveAt(0);
                    levelEditors[i - 1].RemoveAt(0);
                }
                level.RemoveAt(i - 1);
                levelEditors.RemoveAt(i - 1);
            }
            oldX = x;
        }

        if (y > oldY)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = oldY; j < y; j++)
                {
                    level[i].Add(null);
                    levelEditors[i].Add(null);
                }
            }
            oldY = y;
        }
        else if (y < oldY)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = oldY; j > y; j--)
                {
                    level[i].RemoveAt(j - 1);
                    levelEditors[i].RemoveAt(j - 1);
                }
            }
            oldY = y;
        }
    }
}
