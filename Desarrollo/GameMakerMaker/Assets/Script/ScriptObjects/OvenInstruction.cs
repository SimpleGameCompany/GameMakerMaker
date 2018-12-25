using UnityEngine;
using System.Collections;

[System.Serializable]
public class OvenInstruction
{
    [SerializeField]
    public OvenBehaviour.OvenType Type;
    
    public bool Complete = false;
}
