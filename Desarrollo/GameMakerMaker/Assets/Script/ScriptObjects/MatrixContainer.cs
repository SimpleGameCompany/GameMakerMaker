using UnityEngine;
using UnityEditor;

[System.Serializable]
public class MatrixContainer 
{
    public MatrixContainer(int size)
    {
        list = new GameObject[size];
    }
    public GameObject[] list;
}