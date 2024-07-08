using UnityEngine;
using System.Collections.Generic;

public class DreamRealityToggle : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.LeftShift; // Key to toggle dream/reality
    public string dreamTag = "Dream"; // Tag for dream objects
    public string realityTag = "Reality"; // Tag for reality objects

    private List<GameObject> dreamObjects; // List of all dream objects
    private List<GameObject> realityObjects; // List of all reality objects
    private bool isInReality = true; // Initial state is reality

    void Start()
    {
        // Initialize lists
        dreamObjects = new List<GameObject>();
        realityObjects = new List<GameObject>();

        // Find all objects with the dream tag
        GameObject[] allDreamObjects = GameObject.FindGameObjectsWithTag(dreamTag);
        foreach (GameObject dreamObject in allDreamObjects)
        {
            dreamObjects.Add(dreamObject);
            dreamObject.SetActive(false); // Hide dream objects initially
        }

        // Find all objects with the reality tag
        GameObject[] allRealityObjects = GameObject.FindGameObjectsWithTag(realityTag);
        foreach (GameObject realityObject in allRealityObjects)
        {
            realityObjects.Add(realityObject);
            realityObject.SetActive(true); // Show reality objects initially
        }
    }

    void Update()
    {
        // Toggle between dream and reality states
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleState();
        }
    }

    public void ToggleState()
    {
        isInReality = !isInReality; // Toggle the state

        // Enable or disable objects based on the current state
        if (isInReality)
        {
            foreach (GameObject dreamObject in dreamObjects)
            {
                dreamObject.SetActive(false);
            }
            foreach (GameObject realityObject in realityObjects)
            {
                realityObject.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject dreamObject in dreamObjects)
            {
                dreamObject.SetActive(true);
            }
            foreach (GameObject realityObject in realityObjects)
            {
                realityObject.SetActive(false);
            }
        }
    }

    public bool IsInReality()
    {
        return isInReality;
    }

    public void SetRealityState()
    {
        if (!isInReality)
        {
            ToggleState();
        }
    }
}
