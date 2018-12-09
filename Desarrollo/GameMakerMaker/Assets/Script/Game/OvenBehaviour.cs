using UnityEngine;
using System.Collections;

public class OvenBehaviour : Interactuable
{

    // Use this for initialization

    public enum OvenType
    {
        Magic,
        Iron,
        Texture
    }
    public OvenType Type;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool PreAction(PlayerController player)
    {
        Debug.Log("Preaction");
        return true;
    }

    public override void PostAction(PlayerController player)
    {
        Debug.Log("Post Action");
    }
}
