using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageText;
    
    // Keep track of current stage
    private static int currentStage = 1;
    
    // Track if this is the first scene load
    private static bool isFirstLoad = true;

    private void Awake()
    {
        // On first load, initialize to stage 1
        if (isFirstLoad)
        {
            currentStage = 1;
            isFirstLoad = false;
        }
    }

    private void Start()
    {
        // If moving to a new scene after an anomaly was found, increment stage
        bool wasLoadedFromPreviousScene = PlayerPrefs.GetInt("IncrementStage", 0) == 1;
        
        if (wasLoadedFromPreviousScene)
        {
            // Only increment once, then reset the flag
            currentStage++;
            PlayerPrefs.SetInt("IncrementStage", 0);
            PlayerPrefs.Save();
        }
        
        UpdateStageUI();
    }

    public void UpdateStageUI()
    {
        if (stageText != null)
        {
            stageText.text = "Stage " + currentStage;
        }
    }

    // Call this when preparing to load the next scene
    public void PrepareForNextStage()
    {
        // Set a flag to indicate we should increment the stage when the next scene loads
        PlayerPrefs.SetInt("IncrementStage", 1);
        PlayerPrefs.Save();
    }
}