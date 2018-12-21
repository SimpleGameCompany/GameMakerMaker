using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverCintas : MonoBehaviour {

    public List<Material> cintas;
    public float velX =0f;
    public float velY =0.1f;
    float offsetX = 0;
    float offsetY = 0;
    public bool setTo0 = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
         offsetX += (Time.deltaTime * velX);
         offsetY += (Time.deltaTime * velY);
        offsetX %= 1;
        offsetY %= 1;
        foreach (Material c in cintas)
        {
            if (setTo0)
            {
                c.mainTextureOffset = new Vector2(0, 0);
            }
            else
            {
                c.mainTextureOffset = new Vector2(offsetX, offsetY);
            }
            
        }
		
	}

    
}
