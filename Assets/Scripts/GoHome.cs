public class GoHome : GAction
{
    public override string Name => "GoHome";

    public GoHome() : base()
    {
        
       PermittedEntities.Add(EntityType
            .Miner); // It appears that this is overridden by the inspector
        preConditions.AddState(StateType.FoundSomething, 0, false);
        Effects.AddState(StateType.AtHome, 0, false);
    }


    public override bool PrePerform() => true;
    public override bool Perform() => true;
    public override bool PostPerform() => true;
}