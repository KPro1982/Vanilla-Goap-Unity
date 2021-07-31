

// a node in the plan graph to be constructed
public class Node
{
    //the action this node represents in the plan
    public IAction action;

    //how much it cost to get to this node
    public float cost;

    //the parent node this node is connected to
    public Node parent;

    //the state of the environment by the time the
    //action assigned to this node is achieved
    public ListOfStates state;

    // Constructor
    public Node(Node parent, float cost, ListOfStates allStates, IAction action)
    {
        this.parent = parent;
        this.cost = cost;
        state = new ListOfStates(allStates);
        this.action = action;
    }

    // Overloaded Constructor
    public Node(Node parent, float cost, ListOfStates allStates, ListOfStates beliefStates,
        GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        state = new ListOfStates(allStates);

        //as well as the world states add the agents beliefs as states that can be
        //used to match preconditions
        foreach (var s in beliefStates)
        {
            if (!state.HasState(s.Key))
            {
                state.AddState(s);
            }
        }

        this.action = action;
    }

    public string PrintNode()
    {



        string dump = "";
        if (action != null)
        {
            dump += "Action: ";
        
            var pre = "";
            var eff = "";

            GAction ga = (GAction) action;
            foreach (var p in ga.preConditions)
            {
                if (pre != "")
                    pre += ", ";
                pre += p.Key;
            }

            foreach (var e in ga.Effects)
            {
                if (eff != "")
                    eff += ", ";
                eff += e.Key;
            }

            dump += "====  " + ga.Name + "(" + pre + ")(" + eff + ")\n";
        

            dump += "All States: ";
            foreach (var s in state)
            {
                dump += "---: " + s.Key;
            
            }
        }

        return dump;


    }
}