using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private bool isAnomaly = false;
    [SerializeField] private GameObject anomalyObject;
    [SerializeField] private GameObject normalObject;
    
    private bool playerInRange = false;
    private bool hasBeenInteracted = false;
    private AnomalyController anomalyController;

    private void Start()
    {
        // Find the anomaly controller
        anomalyController = Object.FindFirstObjectByType<AnomalyController>();
        
        // Set initial state - anomaly is visible, normal object is hidden
        if (isAnomaly)
        {
            if (anomalyObject != null)
            {
                anomalyObject.SetActive(true);
            }
            
            if (normalObject != null)
            {
                normalObject.SetActive(false);
            }
        }
        else
        {
            // For non-anomaly objects, normal is visible
            if (normalObject != null)
            {
                normalObject.SetActive(true);
            }
            
            if (anomalyObject != null)
            {
                anomalyObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (playerInRange && !hasBeenInteracted && Input.GetKeyDown(KeyCode.Space))
        {
            // Note: We no longer call UseInteraction() here since the AnomalyController
            // is now handling global interactions. We just check if there are interactions left.
            if (anomalyController != null && anomalyController.GetRemainingInteractions() > 0)
            {
                if (isAnomaly)
                {
                    hasBeenInteracted = true;
                    ReplaceAnomalyWithNormal();
                }
            }
        }
    }
    
    private void ReplaceAnomalyWithNormal()
    {
        // Hide the anomaly
        if (anomalyObject != null)
        {
            anomalyObject.SetActive(false);
        }
        
        // Show the normal object
        if (normalObject != null)
        {
            normalObject.SetActive(true);
        }
        
        // Notify the anomaly controller that anomaly was found
        if (anomalyController != null)
        {
            // Pass the anomaly object to the controller
            anomalyController.AnomalyInteraction(anomalyObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}