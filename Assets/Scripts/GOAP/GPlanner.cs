using System.Collections.Generic;
using UnityEngine;

public class GPlanner : MonoBehaviour
{
    public Queue<IAction> queue; 
        
    public Queue<IAction> Plan(List<IAction> actions, SubGoal subGoals, ListOfStates beliefStates)
    {
        var usableActions = new List<IAction>();

        //of all the AllPotentialActions available find the ones that can be achieved.
        foreach (var a in actions)
        {
            if (a.IsAchievable())
            {
                // test tags to see if the particular navAgent type can use the action

                usableActions.Add(a);
            }
        }

        //create the first node in the graph
        // leaves are really PLANS -- Refactor this
        var leaves = new List<Node>();
        ListOfStates allStates = GWorld.Instance.GetWorld();
        var start = new Node(null, 0.0f, allStates, beliefStates, null);
        // Debug.Log("Start Node:" + start.PrintNode());

        //pass the first node through to start branching out the graph of plans from
        var success = BuildGraph(start, leaves, usableActions, subGoals);

        //if a plan wasn't found
        if (!success)
        {
            // Debug.Log("NO PLAN");
            return null;
        }
        PrintLeaves(leaves);

        //of all the plans found, find the one that's cheapest to execute
        //and use that
        Node cheapest = null;
        foreach (var leaf in leaves)
        {
            // a leaf is just a plan

            if (cheapest == null)
            {
                cheapest = leaf;
            }
            else if (leaf.cost < cheapest.cost)
            {
                cheapest = leaf;
            }
        }

        var result = new List<IAction>();
        // cheapest node will be at the end of the action plan
        var n = cheapest;

        while (n != null)
        {
            if (n.action != null)
            {
                // index of 0 means that each node is inserted at top so that the last node gets 
                // pushed down and remains last.
                result.Insert(0, n.action);
            }

            n = n.parent;
        }

        //make a queue out of the AllPotentialActions represented by the nodes in the plan
        //for the navAgent to work its way through
        queue = new Queue<IAction>();

        foreach (IAction a in result)
        {
            // Enqueue puts at end so as not to reverse the order now that it is correct
            queue.Enqueue(a);
        }

        string dump = "The Plan is:\n";
        int i = 0;
        
        foreach (var a in queue)
        {
            
            dump += $"{i++}: {a.Name}\n";
        }
        Debug.Log(dump);

        return queue;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<IAction> usableActions,
        SubGoal subGoals)
    {
        var foundPath = false;
        

        //with all the useable AllPotentialActions
        foreach (var action in usableActions)
        {
            //check their preconditions
            if (action.IsAchievableGiven(parent.state))
            {
                //get the state of the world if the parent node were to be executed
                var currentStates = new ListOfStates(parent.state);

                // build a long list by collecting all of the effect of each action in usable action
                // A long list is required because each element becomes the parent node for a new 
                // attempt to form a plan. Most of the branches following a completed plan are
                // eliminated due to higher cost.

                foreach (var eff in action.Effects)
                {
                    if (!currentStates.HasState(eff.Key))
                    {
                        currentStates.AddState(eff);
                    }
                }

                //create the next node in the branch and set this current node as the parent
                var node = new Node(parent, parent.cost + action.Cost, currentStates, action);
                
                
                
                //if the current state of the world after doing this node's action is the goalList
                //this plan will achieve that goalList and will become the navAgent's plan
                if (GoalAchieved(subGoals, currentStates))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    //if no goalList has been found branch out to add other AllPotentialActions to the plan
                    var subset = ActionSubset(usableActions, action);
                    var found = BuildGraph(node, leaves, subset, subGoals);

                    if (found)
                    {
                        foundPath = true;
                    }
                }
            }
        }

        return foundPath;
    }

    //remove and action from a list of AllPotentialActions
    private List<IAction> ActionSubset(List<IAction> actions, IAction removeMe)
    {
        var subset = new List<IAction>();

        foreach (IAction a in actions)
        {
            if (!a.Equals(removeMe))
            {
                subset.Add(a);
            }
        }

        return subset;
    }

    //check goals against state of the world to determine if the goalList has been achieved.
    private bool GoalAchieved(SubGoal _goalList, ListOfStates states)
    {
        foreach (var s in _goalList)
        {
            if (!states.HasState(s.Key))
            {
                return false;
            }
        }

        return true;
    }

    public void PrintLeaves(List<Node> leaves)
    {
        string dump = "Leaves ---\n";
        foreach (Node n in leaves)
            dump += "--" + n.PrintNode() + "\n";
        Debug.Log(dump);
        Debug.Log("\n");
    }
}