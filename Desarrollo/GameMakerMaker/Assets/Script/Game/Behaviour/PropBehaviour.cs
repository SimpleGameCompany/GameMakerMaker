using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class PropBehaviour : Interactuable {
    public enum PropWorld
    {
        Mario,
        Portal,
        Pacman,
        Pokemon,
        Zelda
    }
    private int tasksCompleted =0;
    private MeshRenderer render;

    public bool Completed { get { return tasksCompleted == recipe.Tasks.Length; } }


    //Esto realmente no se donde meterlo jajaja
    [SerializeField]
    public PropWorld world;
    public Recipe recipe;

    [HideInInspector]
    public NavMeshAgent agent;

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
            render.material = ProcessSkins[tasksCompleted];
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
        agent.enabled = false;
        gameObject.transform.localPosition = Vector3.zero;
        player.anim.SetTrigger(Constantes.ANIMATION_PLAYER_PICK);
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

        render = GetComponentInChildren<MeshRenderer>();
        agent = GetComponent<NavMeshAgent>();
        TasksCompleted = 0;
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
