using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropBehaviour : Interactuable {
    public enum PropWorld
    {
        Mario,
        Portal,
        Pacman
    }
    private int tasksCompleted;
    private MeshRenderer render;

    public bool Completed { get { return tasksCompleted == recipe.Tasks.Length; } }


    //Esto realmente no se donde meterlo jajaja
    [SerializeField]
    public PropWorld world;
    public Recipe recipe;

    [SerializeField]
    Material[] ProcessSkins;
    public int TasksCompleted
    {
        get
        {
            return tasksCompleted;
        }

        set
        {
            tasksCompleted = value;
            if(tasksCompleted > 0)
            {
                if(tasksCompleted == recipe.Tasks.Length)
                {
                   render.material = ProcessSkins[2];
                }
                else
                {
                    render.material = ProcessSkins[1];
                }
            }
            else
            {
                render.material = ProcessSkins[0];
            }
        }
    }

    public override void PostAction(PlayerController player)
    {
        if (this.transform.parent != null)
        {
            TableBehaviour table = this.transform.parent.gameObject.GetComponent<TableBehaviour>();
            if (table != null)
            {
                table.Remove();
            }
        }
        player.PickedObjet = this;
        gameObject.transform.SetParent(player.transform);
        gameObject.transform.localPosition = Vector3.zero;
    }

    public override bool PreAction(PlayerController player)
    {
        if (player.PickedObjet == null)
        {
            return true;
        }
        else
            return false;
    }



    private void Awake()
    {
        recipe = Instantiate(recipe) as Recipe;
        render = GetComponent<MeshRenderer>();
    }

    public void Restart()
    {
        TasksCompleted = 0;
        foreach(var e in recipe.Tasks)
        {
            e.Complete = false;
        }
    }
}
