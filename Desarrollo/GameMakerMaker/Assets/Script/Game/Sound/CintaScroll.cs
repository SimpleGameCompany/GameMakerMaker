using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CintaScroll : MonoBehaviour {

    MeshRenderer render;
    float offset = 0;

    // Use this for initialization
    void Start () {
        render = GetComponent<MeshRenderer>();
    }

    public void Update()
    {
        offset += (Time.deltaTime * 0.25f);
        offset = offset % 1;
        render.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
        render.material.SetTextureOffset("_BumpMap", new Vector2(0, offset));
    }
}
