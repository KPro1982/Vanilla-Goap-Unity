using System.Collections.Generic;
using UnityEngine;


public class GPlanner {

    public Queue<GAction> plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates beliefStates) {

        List<GAction> usableActions = new List<GAction>();

        //of all the actions available find the ones that can be achieved.
        foreach (GAction a in actions) {

            if (a.IsAchievable()) {        // test tags to see if the particular agent type can use the action

                usableActions.Add(a);
            }
        }

        //create the first node in the graph
        // leaves are really PLANS -- Refactor this
        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0.0f, GWorld.Instance.GetWorld().GetStates(), beliefStates.GetStates(), null);

        //pass the first node through to start branching out the graph of plans from
        bool success = BuildGraph(start, leaves, usableActions, goal);

        //if a plan wasn't found
        if (!success) {

            Debug.Log("NO PLAN");
            return null;
        }

        //of all the plans found, find the one that's cheapest to execute
        //and use that
        Node cheapest = null;
        foreach (Node leaf in leaves) {   // a leaf is just a plan

            if (cheapest == null) {

                cheapest = leaf; 
            } else if (leaf.cost < cheapest.cost) {

                cheapest = leaf;
            }
        }
        List<GAction> result = new List<GAction>();
        // cheapest node will be at the end of the action plan
        Node n = cheapest;

        while (n != null) {

            if (n.action != null) {
                // index of 0 means that each node is inserted at top so that the last node gets 
                // pushed down and reamins last.
                result.Insert(0, n.action);
            }

            n = n.parent;
        }

        //make a queue out of the actions represented by the nodes in the plan
        //for the agent to work its way through
        Queue<GAction> queue = new Queue<GAction>();

        foreach (GAction a in result) {
            // Enqueue puts at end so as not to reverse the order now that it is correct
            queue.Enqueue(a);
        }

        Debug.Log("The Plan is: ");
        foreach (GAction a in queue) {

            Debug.Log("Q: " + a.actionName);
        }

        return queue;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<string, int> goal) {

        bool foundPath = false;

        //with all the useable actions
        foreach (GAction action in usableActions) {

            //check their preconditions
            if (action.IsAchievableGiven(parent.state)) {

                //get the state of the world if the parent node were to be executed
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);

               // build a long list by collecting all of the effect of each action in usable action
               // BUT WHY do we need the long list. Don't we just need the last one in the chain to 
               // match up to the next one?  What happens if an action matches one of the states that 
               // already matched. Example foodOnRoute could match pizzaOrdered and UberEatsOrdered.
               
               foreach (KeyValuePair<string, int> eff in action.effects) {

                    if (!currentState.ContainsKey(eff.Key)) {

                        currentState.Add(eff.Key, eff.Value);
                    }
                }

                //create the next node in the branch and set this current node as the parent
                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                //if the current state of the world after doing this node's action is the goal
                //this plan will achieve that goal and will become the agent's plan
                if (GoalAchieved(goal, currentState)) {

                    leaves.Add(node);
                    foundPath = true;
                } else {
                    //if no goal has been found branch out to add other actions to the plan
                    List<GAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);

                    if (found) {

                        foundPath = true;
                    }
                }
            }
        }
        return foundPath;
    }

    //remove and action from a list of actions
    private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe) {

        List<GAction> subset = new List<GAction>();

        foreach (GAction a in actions) {

            if (!a.Equals(removeMe)) {

                subset.Add(a);
            }
        }
        return subset;
    }

    //check goals against state of the world to determine if the goal has been achieved.
    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state) {

        foreach (KeyValuePair<string, int> g in goal) {

            if (!state.ContainsKey(g.Key)) {

                return false;
            }
        }
        return true;
    }
}
