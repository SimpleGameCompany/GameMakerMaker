using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCFuncionando : MonoBehaviour {

    public List<Material> Mats_Funcionamiento;
    public Material Mat_Apagado;
    private int currentMaterial = 0;
	// Use this for initialization
	void Start () {
        encenderPC(0.15f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void encenderPC(float intervalo)
    {
        InvokeRepeating("nextMaterial", 0.1f,intervalo);        
    }
    public void pararPC()
    {
        GetComponent<Renderer>().material = Mat_Apagado;
    }
    private void nextMaterial()
    {
        if (currentMaterial >= Mats_Funcionamiento.Count)
            currentMaterial = 0;
        GetComponent<Renderer>().material = Mats_Funcionamiento[currentMaterial];
        currentMaterial++;
        Debug.Log(currentMaterial);
        


    }
}
