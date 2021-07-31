using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class State 
{
    public string Name;
    public StateType Key;
    public int Value;
    private readonly List<GAction> AssignedAsEffect = new();
    private readonly List<GAction> AssignedAsPrecondition = new();


    private readonly List<GAgent> OwnedBy = new();

    public State(StateType _state, int _value)
    {
        Key = _state;
        Value = _value;
        Name = _state.ToString();
    }

    public bool IsOwned => OwnedBy.Count == 0;

    public bool IsEffect => AssignedAsEffect.Count == 0;

    public bool IsPrecondition => AssignedAsPrecondition.Count == 0;

    public bool IsMatched => IsOwned && IsPrecondition && IsEffect;

    public void AssignOwner(GAgent _owner)
    {
        OwnedBy.Add(_owner);
    }

    public void AssignPrecondition(GAction _action)
    {
        AssignedAsPrecondition.Add(_action);
    }

    public void AssignEffect(GAction _action)
    {
        AssignedAsEffect.Add(_action);
    }
}