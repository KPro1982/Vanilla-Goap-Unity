public class Capture : GAction
{
    public override string Name => "Capture";

    public Capture() : base()
    {
        
        PermittedEntities.Add(EntityType.Miner); // It appears that this is overridden by the inspector
        preConditions.AddState(StateType.Searching, 0, false);
        Effects.AddState(StateType.FoundSomething, 0, false);
    }


    public override bool PrePerform() => true;
    public override bool Perform() => true;
    public override bool PostPerform() => true;
}