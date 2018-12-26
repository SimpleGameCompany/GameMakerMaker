using UnityEngine;
using System.Collections;

public class Floor : Interactuable
{
    public override void PostAction(PlayerController player)
    {
        
    }

    public override bool PreAction(PlayerController player)
    {
        return true;
    }

    public override bool HasToStare()
    {
        return false;
    }

}
