using System;
using System.Collections;
using System.Collections.Generic;

//make the dictionary elements their own serializable class
//so we can edit them in the inspector
[Serializable]
public class ListOfStates : IEnumerable<State>
{
    // Constructor
    public List<State> states;

    public ListOfStates()
    {
        states = new List<State>();
    }

    public ListOfStates(ListOfStates source)
    {
        states = new List<State>();
        foreach (var s in source)
        {
            states.Add(s);
        }
    }

    public ListOfStates(StateType s, int i)
    {
        states = new List<State>();
        states.Add(new State(s, i));
    }

    /*public ListOfStates GetStates() {

        return states;
    }*/

    public IEnumerator<State> GetEnumerator() => states.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public bool HasState(StateType key)
    {
        return states.Exists(k => k.Key == key); 
        // r = false;
        // foreach (State s in states)  // TODO Delete commented code after lambda is verified
        // {
        //     if (s.Key == key)
        //     { 
        //         r = true;
        //     }
        // }
        // return r;
    }

    // Add to our list
    public State AddState(StateType key, int value, bool removable)
    {
        var s = new State(key, value);
        states.Add(s);
        return s;
    }

    public void AddState(State s)
    {
        states.Add(s);
    }

    public void ModifyState(StateType key, int value, bool removable)
    {
        // If it contains this Key
        if (HasState(key))
        {
            // Add the Value to the state
            states.Find(k => k.Key == key).Value += value;

            // If it's less than zero then remove it
            if (states.Find(k => k.Key == key).Value <= 0)
            {
                // Call the RemoveState method
                RemoveState(key);
            }
        }
        else
        {
            AddState(key, value, removable);
        }
    }

    // Method to remove a state
    public void RemoveState(StateType key)
    {
        // Check first if it exists
        if (HasState(key))
        {
            states.RemoveAll(k => k.Key == key);
        }
    }

    // Set a state
    public void SetState(StateType key, int value, bool r)
    {
        // Check if it exists
        if (HasState(key))
        {
            states.Find(k => k.Key == key).Value = value;
        }
        else
        {
            AddState(key, value, r);
        }
    }
}