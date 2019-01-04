using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Zelda,
        Minecraft
    }
    private int tasksCompleted = 0;
    private MeshRenderer render;

    public bool Completed { get { return tasksCompleted == recipe.Tasks.Length; } }

    public ParticleSystem glow;


    //Esto realmente no se donde meterlo jajaja
    [SerializeField]
    public PropWorld world;
    public Recipe recipe;

    [HideInInspector]
    public NavMeshAgent agent;

    [SerializeField]
    Material[] ProcessSkins;

    [HideInInspector]
    public Animator anim;

    [HideInInspector]
    public bool grab;

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
            if(tasksCompleted == ProcessSkins.Length - 1)
            {
                glow.Play(true);
            }
        }
    }

    public override void PostAction(PlayerController player)
    {
        if (grab)
        {
            if (this.transform.parent != null)
            {
                TableBehaviour table = this.transform.parent.gameObject.GetComponent<TableBehaviour>();
                if (table != null)
                {
                    table.Remove();
                }
            }
            player.interacting = true;
            player.anim.SetTrigger(Constantes.ANIMATION_PLAYER_PICK);
            agent.enabled = false;
            
        }
    }

    public override void PostActionAnim(PlayerController player)
    {
        base.PostAction(player);
        player.PickedObjet = this;
        gameObject.transform.SetParent(player.grabPoint);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.rotation = Quaternion.Euler(0,0,0);
        anim.SetBool(Constantes.ANIMATION_PROP_GRAB, true);
        player.interacting = false;
    }

    public override bool PreAction(PlayerController player)
    {
        
        if (player.PickedObjet == null)
        {
            base.PreAction(player);
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
        anim = GetComponent<Animator>();
        TasksCompleted = 0;
    }

    public void Restart()
    {
        TasksCompleted = 0;
        foreach (var e in recipe.Tasks)
        {
            e.Complete = false;
        }
    }

    public void Eliminate()
    {
        GameManager.Instance.StoreProp(gameObject);
        LifeController.Instance.Lifes--;
    }


    public void SetNavMeshDestination()
    {
       agent.SetDestination(GameManager.Instance.EndPosition);
    }


    public override bool UpdatePosition(PlayerController player)
    {
        return gameObject.activeSelf &&  player.SetDestination(transform.position);
    }

    internal void DesactiveBehaviour()
    {
        foreach(var e in GameManager.Instance.ovens)
        {
            if (e.Indicator.activeSelf)
            {
                e.Indicator.SetActive(false);
            }
        }
    }


    public void Activate()
    {
        OvenBehaviour.OvenType[] completedTypes = (from x in recipe.Tasks where x.Complete select x.Type).ToArray();
        OvenBehaviour[] toactivate = (from x in GameManager.Instance.ovens where completedTypes.Contains(x.Type) select x).ToArray();

        foreach(var e in toactivate)
        {
            e.Indicator.SetActive(true);
        }


    }
}
