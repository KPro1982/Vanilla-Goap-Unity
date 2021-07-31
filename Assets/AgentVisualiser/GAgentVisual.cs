using UnityEngine;

//The class to link the GAgent code with the Inspector Editor so the navAgent's
//properties can be displayed in the Inspector
[ExecuteInEditMode]
public class GAgentVisual : MonoBehaviour
{
    public GAgent thisAgent;

    // Start is called before the first frame update
    private void Start()
    {
        thisAgent = GetComponent<GAgent>();
    }

    // Update is called once per frame
    private void Update()
    {
    }
} 