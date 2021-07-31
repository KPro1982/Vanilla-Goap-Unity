public class SubGoal : ListOfStates
{
    public bool Removeable;
    // Bool to store if goal should be removed after it has been achieved


    // Constructor
    public SubGoal(StateType s, int i, bool removeable) : base(s, i)
    {
        Removeable = removeable;
    }

    public void SetRemovable(bool r)
    {
        Removeable = r;
    }

    public bool IsRemovable() => Removeable;
}