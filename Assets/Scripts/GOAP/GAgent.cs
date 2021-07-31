using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAgent : MonoBehaviour
{
    // Our local states
    public ListOfStates entityStates = new();

    // Action Queue
    public Queue<IAction> _actionQueue;

    // Our subgoal
    private SubGoal _currentGoal;

    // Access the planner
    private GPlanner _planner;
    

    // Store our list of AllPotentialActions
    public List<IAction> AllPotentialActions = new();

    // Our current action
    public IAction currentAction;

    protected EntityType entityType;
    private NavMeshAgent NavAgent;

    // List of agent's goals
    public List<SubGoal> goals = new();

    // Our inventory
    public GInventory inventory = new();

    private bool invoked;


    public virtual void Awake()
    {
        AllPotentialActions = ActionFactory.GetActionsByEntityType(entityType);
        NavAgent = GetComponent<NavMeshAgent>();
    }

    public virtual void Start()
    {
    }

    private void LateUpdate()
    {
        //if there's a current action and it is still IsRunning
        if (currentAction != null && currentAction.IsRunning)
        {
            // Find the distance to the Target
            var distanceToTarget = Vector3.Distance(currentAction.Target.transform.position,
                transform.position);
            // Check the navAgent has a goal and has reached that goal
            if (NavAgent.hasPath && distanceToTarget < 2.0f)
            {
                
                if (!invoked)
                {
                    //if the action movement is complete wait
                    //a certain duration for it to be completed
                    Invoke("CompleteAction", currentAction.Duration);  //TODO move this logic within the GAction class
                    invoked = true;
                }
            }

            return;
        }

        // Check we have a planner and an actionQueue
          if (_planner == null || _actionQueue == null)
        {
            // If planner is null then create a new one
            _planner = new GPlanner();

            // Sort the goals in descending order and store them in sortedGoals
            // Skipping this step with unknown consequences


            //look through each goal to find one that has an achievable plan
            foreach (var sg in goals)
            {
                _actionQueue = _planner.Plan(AllPotentialActions, sg, entityStates);
                // If actionQueue is not = null then we must have a plan
                if (_actionQueue != null)
                {
                    // Set the current goal
                    _currentGoal = sg;
                    break;
                }
            }
        }

        // Have we an actionQueue
        if (_actionQueue != null && _actionQueue.Count == 0)
        {
            // Check if currentGoal is Removable
            if (_currentGoal.Removeable)
            {
                // Remove it
                goals.Remove(_currentGoal);
            }

            // Set planner = null so it will trigger a new one
            _planner = null;
        }

        // Do we still have AllPotentialActions
        if (_actionQueue != null && _actionQueue.Count > 0)
        {
            // Remove the top action of the queue and put it in currentAction
            currentAction = _actionQueue.Dequeue();

            if (currentAction.PrePerform())
            {
                MoveToTarget();
            }
            else
            {
                // Force a new plan
                _actionQueue = null;
            }
        }
    }

    private void MoveToTarget()
    {
        
        if (currentAction.Target != null)
        {
            // Activate the current action
            currentAction.IsRunning = true;
            // Pass Unities AI the destination for the navAgent
            NavAgent.SetDestination(currentAction.Target.transform.position);
        }
    }

    //an invoked method to allow an navAgent to be performing a task
    //for a set location
    public void CompleteAction()
    {
        currentAction.IsRunning = false;
        currentAction.PostPerform();
        invoked = false;
    }
}

public enum EntityType
{
    None,
    Nurse,
    Miner
}