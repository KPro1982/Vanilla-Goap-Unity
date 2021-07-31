using UnityEngine;

public class Explore : GAction
{
    public override string Name => "Explore";

    public Explore() : base()
    {
        
        PermittedEntities.Add(EntityType.Miner); // It appears that this is overridden by the inspector
        preConditions.AddState(StateType.Idle, 0, false);
        Effects.AddState(StateType.Searching, 0, false);
        Target = GameObject.FindGameObjectWithTag("ExploreTarget");

    }


    public override bool PrePerform() => true;
    public override bool Perform() => true;
    public override bool PostPerform() => true;
}