using UnityEngine;
using UnityEngine.UI;

public class UpdateWorld : MonoBehaviour
{
    // Storage for the states
    public Text states;

    private void LateUpdate()
    {
        // Dictionary of states
        var worldStates = GWorld.Instance.GetWorld();
        // Clear out the states text
        states.text = "";
        // Cycle through them all and store in states.text
        foreach (var s in worldStates)
        {
            states.text += s.Key + ", " + s.Value + "\n";
        }
    }
}