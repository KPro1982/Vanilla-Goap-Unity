public class Miner : GAgent
{
    public override void Awake()
    {
        
        entityType = EntityType.Miner;
        base.Awake();
        SubGoal sg = new SubGoal(StateType.AtHome, 0, true);
        goals.Add(sg);
        entityStates.AddState(StateType.Idle, 0, true);
    }
}