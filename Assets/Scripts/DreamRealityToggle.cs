using UnityEngine;
using System.Collections.Generic;

public class DreamRealityToggle : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.LeftShift; 
    public string dreamTag = "Dream"; 
    public string realityTag = "Reality"; 

    private List<GameObject> dreamObjects; 
    private List<GameObject> realityObjects; 
    private bool isInReality = true; 

    void Start()
    {
        dreamObjects = new List<GameObject>();
        realityObjects = new List<GameObject>();

        GameObject[] allDreamObjects = GameObject.FindGameObjectsWithTag(dreamTag);
        foreach (GameObject dreamObject in allDreamObjects)
        {
            dreamObjects.Add(dreamObject);
            dreamObject.SetActive(false);
        }

        GameObject[] allRealityObjects = GameObject.FindGameObjectsWithTag(realityTag);
        foreach (GameObject realityObject in allRealityObjects)
        {
            realityObjects.Add(realityObject);
            realityObject.SetActive(true); 
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
        isInReality = !isInReality;

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
