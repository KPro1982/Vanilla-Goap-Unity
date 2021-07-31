using System.Collections.Generic;
using UnityEngine;

public sealed class GLibrary : MonoBehaviour
{
    private GLibrary _instance = null;
    private GLibrary()
    {
    } // private  constructor is required by Singleton pattern

    public  GLibrary Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GLibrary();
            }

            return _instance;
        }
    }

    public void Awake()
    {
    }

    private static List<T> ListFromArray<T>(T[] array)
    {
        var result = new List<T>();
        foreach (var item in array)
        {
            result.Add(item);
        }

        return result;
    }
}