using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The class to link the GPlanner code with the Inspector Editor 
[ExecuteInEditMode]
public class PlannerLink : MonoBehaviour
{
    public GPlanner thisPlanner;

    // Start is called before the first frame update
    private void Start()
    {
        GAgent thisAgent = FindObjectOfType<GAgent>();
        thisPlanner = thisAgent.GetComponent<GPlanner>();
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
