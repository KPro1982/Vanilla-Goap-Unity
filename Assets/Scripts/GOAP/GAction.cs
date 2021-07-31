using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction :  IAction
{
    // Name of the action

    // Cost of the action
    public List<EntityType> permittedEntities;

    public float cost = 1.0f;

    // Target where the action is going to take place
    public GameObject target;

    // Store the tag
    public string targetTag;

    // Duration the action should take
    public float duration;

    // An array of GStates of preconditions
    public ListOfStates preConditions;

    // An array of GStates of effects
    public ListOfStates effects;
    
    // State of the Agent
    public ListOfStates agentBeliefs;

    public ListOfStates beliefs;

    // Are we currently performing an action?
    public bool isRunning;

    // Access our inventory
    public GInventory inventory;


    // Constructor
    protected GAction()
    {
        PermittedEntities = new List<EntityType>();
        agentBeliefs = new ListOfStates();
        preConditions = new ListOfStates();
        Effects = new ListOfStates();
        inventory = new GInventory();
    }

    public bool IsRunning
    {
        get => isRunning;
        set => isRunning = value;
    }

    public GameObject Target
    {
        get => target;
        set => target = value;
    }

    public string TargetTag
    {
        get => targetTag;
        set => targetTag = value;
    }

   
    public float Cost
    {
        get => cost;
        set => cost = value;
    }

    public float Duration
    {
        get => duration;
        set => duration = value;
    }

    public virtual string Name { get; }

    public ListOfStates Effects
    {
        get => effects;
        set => effects = value;
    }

    public List<EntityType> PermittedEntities
    {
        get => permittedEntities;
        set => permittedEntities = value;
    }


    public bool
        IsAchievable() => // test tags to see if the particular navAgent type can use the action
        true;

    //check if the action is achievable given the condition of the
    //world and trying to match with the AllPotentialActions preconditions
    public bool IsAchievableGiven(ListOfStates conditions)
    {
        foreach (var p in preConditions)
        {
            if (!conditions.HasState(p.Key))
            {
                return false;
            }
        }

        return true;
    }

    public abstract bool PrePerform();
    public abstract bool Perform();
    public abstract bool PostPerform();

}