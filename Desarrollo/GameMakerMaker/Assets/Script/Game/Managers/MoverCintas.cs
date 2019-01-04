using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverCintas : MonoBehaviour {

    public List<Material> cintas;
    
    public float velX =0f;
    public float velY =0.1f;
    public List<float> offsetsY = new List<float>(0);//para poder añadir un offset a cada material.
    float offsetX = 0;
    float offsetY = 0;
    public bool setTo0 = false;
    private GameObject[] rodillos;
	// Use this for initialization
	void Start () {
        rodillos = GameObject.FindGameObjectsWithTag("Rodillo");
		
	}
	
	// Update is called once per frame
	void Update () {
       
         offsetX += (Time.deltaTime * velX);
         offsetY += (Time.deltaTime * velY);
        offsetX %= 1;
        offsetY %= 1;

        int i = 0;//Index pa el forEach
        foreach (Material c in cintas)
        {
            if (setTo0)
            {
                c.mainTextureOffset = new Vector2(0, 0);
            }
            else
            {
                c.mainTextureOffset = new Vector2(offsetX, offsetY+offsetsY[i]);
            }
            i++;
            
        }
        
        foreach(GameObject rodillo in rodillos)
        {
            rodillo.transform.Rotate(new Vector3(0.0f, 0.0f, velY * 200*Time.deltaTime));
        }
		
	}

    
}
