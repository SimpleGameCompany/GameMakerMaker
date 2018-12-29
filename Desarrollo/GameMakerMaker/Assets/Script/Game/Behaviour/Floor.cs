using UnityEngine;
using System.Collections;

public class Floor : Interactuable
{
    public override void PostAction(PlayerController player)
    {
        base.PostAction(player);
    }

    public override void PostActionAnim(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public override bool PreAction(PlayerController player)
    {
        base.PreAction(player);
        player.MarkObject.transform.position = player.hit.point;
        return true;
    }

    public override bool HasToStare()
    {
        return false;
    }

}
