using System.Collections.Generic;
using UnityEngine;

public sealed class GWorld
{
    // Our GWorld instance

    // Our _g states
    private static readonly ListOfStates _g;

    // Queue of patients
    private static readonly Queue<GameObject> patients;

    // Queue of cubicles
    private static readonly Queue<GameObject> cubicles;

    // List of Actions
    private static List<GAction> actionLibrary;


    static GWorld()
    {
        // Create our _g
        _g = new ListOfStates();
        // Create patients array
        patients = new Queue<GameObject>();
        // Create cubicles array
        cubicles = new Queue<GameObject>();
        // Find all GameObjects that are tagged "Cubicle"
        // var cubes = GameObject.FindGameObjectsWithTag("Cubicle");
        // Then add them to the cubicles Queue
        /*foreach (var c in cubes)
        {
            cubicles.Enqueue(c);
        }*/

        // Inform the state
        /*if (cubes.Length > 0)
        {
            // _g.ModifyState(StateType.FreeCubicle, cubes.Length, true);
        }

        // Set the time scale in Unity
        Time.timeScale = 5.0f;*/
    }

    private GWorld()
    {
    }

    public static GWorld Instance { get; } = new();

    // Add patient
    public void AddPatient(GameObject p)
    {
        // Add the patient to the patients Queue
        patients.Enqueue(p);
    }

    // Remove patient
    public GameObject RemovePatient()
    {
        if (patients.Count == 0)
        {
            return null;
        }

        return patients.Dequeue();
    }
    // TODO reintroduce resource and inventory checking
    // Add cubicle
    // public void AddCubicle(GameObject p)
    // {
    //     // Add the patient to the patients Queue
    //     cubicles.Enqueue(p);
    // }
    //
    // // Remove cubicle
    // public GameObject RemoveCubicle()
    // {
    //     // Check we have something to remove
    //     if (cubicles.Count == 0)
    //     {
    //         return null;
    //     }
    //
    //     return cubicles.Dequeue();
    // }

    public ListOfStates GetWorld() => _g;
}

