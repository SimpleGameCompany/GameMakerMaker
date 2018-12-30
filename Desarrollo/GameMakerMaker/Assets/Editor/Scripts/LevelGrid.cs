using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class LevelGrid 
{
    static Vector2 scrollPos;

    static int x = 1;
    static int y = 1;

    static int oldX;
    static int oldY = 1;

    static Material levelFloor = null;

    static List<List<GameObject>> level = new List<List<GameObject>>();
    static List<List<GameObject>> levelObjects = new List<List<GameObject>>();
    static GameObject floor;
    static GameObject floorPref;
    GameObject layout;

    public LevelGrid()
    {
        layout = GameObject.FindGameObjectWithTag(Constantes.TAG_LAYOUT);
       Debug.Log("Hello");
    }



    public void OnGUI()
    {

        #region Dimensiones
        GUILayout.Label("Dimensiones", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("-", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(20f)) && x > 1)
        {
            x -= 2;
        }
        GUI.enabled = false;
        x = EditorGUILayout.IntField(x, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(20f));
        GUI.enabled = true;
        if (GUILayout.Button("+", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(20f)))
        {
            x += 2;
        }

        if (GUILayout.Button("-", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(20f)) && y > 1)
        {
            y -= 2;
        }
        GUI.enabled = false;
        y = EditorGUILayout.IntField(y, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(20f));
        GUI.enabled = true;
        if (GUILayout.Button("+", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(20f)))
        {
            y += 2;
        }

        GUILayout.EndHorizontal();
        #endregion

        #region Suelo
        GUILayout.Label("Suelo", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        EditorGUI.BeginChangeCheck();
        floorPref = (GameObject)EditorGUILayout.ObjectField(floorPref, typeof(GameObject), false, GUILayout.ExpandWidth(false), GUILayout.MinWidth(160f), GUILayout.MaxWidth(160f));
        if (EditorGUI.EndChangeCheck())
        {
            if (floorPref != null)
            {
                AddFloor(floorPref);
            }
            else
            {
                RemoveFloor();
            }
        }
        EditorGUI.BeginChangeCheck();
        levelFloor = (Material)EditorGUILayout.ObjectField(levelFloor, typeof(Material), false, GUILayout.ExpandWidth(false), GUILayout.MinWidth(160f), GUILayout.MaxWidth(160f));
        if (EditorGUI.EndChangeCheck())
        {
            if (levelFloor != null)
            {
                ChangeMaterial(levelFloor);
            }
            else
            {
                RemoveMaterial();
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        #endregion

        ChangeGrid();

        #region Grid
        GUILayout.Label("Level", EditorStyles.boldLabel);
        for (int j = 0; j < y; j++)
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false), GUILayout.MaxWidth(320f));
            for (int i = 0; i < x; i++)
            {
                EditorGUI.BeginChangeCheck();
                level[i][j] = (GameObject)EditorGUILayout.ObjectField(level[i][j], typeof(GameObject), false, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(60f),GUILayout.MinWidth(60f), GUILayout.MaxHeight(40f), GUILayout.MinHeight(40f));
                if (EditorGUI.EndChangeCheck())
                {
                    if(level[i][j] == null)
                    {
                        RemoveObject(i,j);
                    }
                    else
                    {
                        AddObject(i, j, level[i][j]);
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        #endregion

    }

    void ChangeGrid()
    {
        if (x > oldX)
        {
            for (int i = oldX; i < x; i++)
            {

                level.Add(new List<GameObject>());
                levelObjects.Add(new List<GameObject>());
                for (int j = 0; j < y; j++)
                {
                    level[i].Add(null);
                    levelObjects[i].Add(null);
                }
            }
            Regenerate(oldX, oldY);
            oldX = x;
        }
        else if (x < oldX)
        {
            for (int i = oldX; i > x; i--)
            {
                for (int j = 0; j < y; j++)
                {
                    level[i - 1].RemoveAt(0);
                    RemoveObject(i-1,0);
                    levelObjects[i - 1].RemoveAt(0);
                }
                level.RemoveAt(i - 1);
                levelObjects.RemoveAt(i - 1);
            }
            Regenerate(x, y);
            oldX = x;
        }

        if (y > oldY && y > 1)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = oldY; j < y; j++)
                {
                    level[i].Add(null);
                    levelObjects[i].Add(null);
                }
            }
            Regenerate(oldX, oldY);
            oldY = y;
        }
        else if (y < oldY)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = oldY; j > y; j--)
                {
                    level[i].RemoveAt(j - 1);
                    RemoveObject(i, j - 1);
                    levelObjects[i].RemoveAt(j - 1);
                }
            }
            Regenerate(x, y);
            oldY = y;
        }
    }

    void AddObject(int posX, int posY, GameObject prefab)
    {
        Object.DestroyImmediate(levelObjects[posX][posY]);
        levelObjects[posX][posY] = Object.Instantiate(prefab, new Vector3(posX-(x/2), 1, -1*(posY-(y/2))), prefab.transform.rotation,layout.transform);
    }

    void RemoveObject(int x, int y)
    {
        Object.DestroyImmediate(levelObjects[x][y]);
    }

    void Regenerate(int posX, int posY)
    {
        for(int i= 0; i< posX; i++)
        {
            for (int j = 0; j < posY; j++)
            {
                if (level[i][j] != null)
                {
                    Object.DestroyImmediate(levelObjects[i][j]);
                    levelObjects[i][j] = Object.Instantiate(level[i][j], new Vector3(i - (x / 2), 1, -1*(j - (y / 2))), level[i][j].transform.rotation, layout.transform);
                }
            }
        }
        if (floor != null)
        {
            floor.transform.localPosition = new Vector3(0, 0, 0);
            floor.transform.localScale = new Vector3(x, 1, y);
        }
    }

    void ChangeMaterial(Material mat)
    {
        if (floor != null)
        {
            floor.GetComponent<MeshRenderer>().material = mat;
        }
    }

    void RemoveMaterial()
    {
        if (floor != null)
        {
            floor.GetComponent<MeshRenderer>().material = null;
        }
    }

    void AddFloor(GameObject fp)
    {
        Object.DestroyImmediate(floor);
        floor = Object.Instantiate(fp, new Vector3(0,0,0), Quaternion.identity, layout.transform);
        floor.transform.localScale = new Vector3(x, 1, y);
    }

    void RemoveFloor()
    {
        Object.DestroyImmediate(floor);
    }

    public void Load(Level l)
    {
        layout = GameObject.FindGameObjectWithTag(Constantes.TAG_LAYOUT);
        Object.DestroyImmediate(layout);
        layout = new GameObject();
        layout.name = "Layout";
        layout.tag = Constantes.TAG_LAYOUT;
        layout.transform.SetParent(GameObject.FindGameObjectWithTag(Constantes.LEVEL_TAG).transform);

        level = (new List<GameObject>[l.Layout.Length]).ToList();
        x = level.Count;
        y = l.Layout[0].list.Length;
        for (int i = 0; i < x; i++)
        {
            level[i] = (new GameObject[y]).ToList();
        }

        

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                level[i][j] = l.Layout[i].list[j];
            }
        }

        
        oldX = x;
        oldY = y;
        floorPref = l.floor;
        AddFloor(floorPref);
        levelObjects = (new List<GameObject>[x]).ToList();
        for(int i = 0; i < x; i++)
        {
            levelObjects[i] = (new GameObject[y]).ToList();
        }

        Regenerate(x, y);




    }

   public Level Save(Level l)
    {
        l.floor = floorPref;
        l.FloorMaterial = levelFloor;
        l.Layout = new MatrixContainer[x];
        


        for(int i = 0; i < x; i++)
        {
            l.Layout[i] = new MatrixContainer(y);
            for(int j =0;j<y;j++)
            {
                l.Layout[i].list[j] = level[i][j];
            }
        }
        return l;
    }
}
