using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IAction
{
    bool IsRunning { get; set; }
    GameObject Target { get; set; }
    float Duration { get; set; }
    string TargetTag { get; set; }
    string Name { get; }
    ListOfStates Effects { get; set; }
    float Cost { get; set; }
    List<EntityType> PermittedEntities { get; set; }

    public bool IsAchievable();
    public bool IsAchievableGiven(ListOfStates conditions);
    public bool PrePerform();
    public bool Perform();
    public bool PostPerform();
}