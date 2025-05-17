using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnomalyController : MonoBehaviour
{
    [Header("Anomaly Settings")]
    [SerializeField] private bool hasAnomaly = true;
    [SerializeField] private List<GameObject> anomalyObjects = new List<GameObject>();
    
    [Header("Interaction Settings")]
    [SerializeField] private int maxInteractions = 3;
    private int remainingInteractions;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI interactionCountText;
    
    private bool anomalyFound = false;

    private void Start()
    {
        // Initialize interactions
        remainingInteractions = maxInteractions;
        UpdateInteractionUI();
        
        // If there are no anomalies or anomaly is disabled, mark as no anomaly
        if (!hasAnomaly || anomalyObjects.Count == 0)
        {
            hasAnomaly = false;
        }
    }

    void Update()
    {
        // Global interaction handler - decreases counter for any E press
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Decrement interaction counter even if not interacting with a specific object
            UseInteraction();
        }
    }

    public bool HasAnomaly()
    {
        return hasAnomaly;
    }

    public bool UseInteraction()
    {
        if (remainingInteractions > 0)
        {
            remainingInteractions--;
            UpdateInteractionUI();
            return true;
        }
        
        return false;
    }

    public void UpdateInteractionUI()
    {
        if (interactionCountText != null)
        {
            interactionCountText.text = "Interactions: " + remainingInteractions;
        }
        else
        {
            // Try to find the UI element if not assigned
            GameObject interactionUI = GameObject.FindGameObjectWithTag("InteractionUI");
            if (interactionUI != null)
            {
                interactionCountText = interactionUI.GetComponent<TextMeshProUGUI>();
                if (interactionCountText != null)
                {
                    interactionCountText.text = "Interactions: " + remainingInteractions;
                }
            }
        }
    }

    public void AnomalyInteraction(GameObject anomalyObject)
    {
        // Check if the provided anomaly object is in our list of anomaly objects
        bool isValidAnomaly = false;
        foreach (GameObject obj in anomalyObjects)
        {
            if (obj == anomalyObject)
            {
                isValidAnomaly = true;
                break;
            }
        }

        if (isValidAnomaly && !anomalyFound)
        {
            anomalyFound = true;
            
            // Log that an anomaly was found
            Debug.Log("Anomaly discovered!");
            
            // Notify the game manager that the anomaly was found
            if (Object.FindFirstObjectByType<GameManager>() != null)
            {
                Object.FindFirstObjectByType<GameManager>().AnomalyDiscovered();
            }
        }
    }

    public int GetRemainingInteractions()
    {
        return remainingInteractions;
    }
    
    public bool IsAnomalyFound()
    {
        return anomalyFound;
    }
}