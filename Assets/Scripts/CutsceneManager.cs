using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [Header("Cutscene Settings")]
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float textDisplayDuration = 3.0f;
    [SerializeField] private float textFadeInDuration = 1.0f;
    [SerializeField] private float textFadeOutDuration = 1.0f;
    
    [Header("UI References")]
    [SerializeField] private Image blackOverlay;
    [SerializeField] private TextMeshProUGUI cutsceneText;
    
    [Header("Cutscene Content")]
    [SerializeField] private List<string> cutsceneMessages = new List<string>();
    
    [Header("Scene References")]
    [SerializeField] private string targetSceneName;
    
    private int currentMessageIndex = 0;
    
    private void Start()
    {
        // Make sure we have all required components
        if (blackOverlay == null || cutsceneText == null)
        {
            Debug.LogError("Missing required UI components for cutscene!");
            SkipToTargetScene();
            return;
        }
        
        // Initialize UI
        blackOverlay.color = new Color(0, 0, 0, 1); // Start fully black
        cutsceneText.color = new Color(1, 1, 1, 0); // Text starts invisible
        
        // Start the cutscene sequence
        StartCoroutine(PlayCutscene());
    }
    
    private IEnumerator PlayCutscene()
    {
        // Small delay before starting
        yield return new WaitForSeconds(0.5f);
        
        // Display each message in sequence
        while (currentMessageIndex < cutsceneMessages.Count)
        {
            // Set the current message
            cutsceneText.text = cutsceneMessages[currentMessageIndex];
            
            // Fade in the text
            yield return StartCoroutine(FadeTextIn());
            
            // Wait for the text display duration
            yield return new WaitForSeconds(textDisplayDuration);
            
            // Fade out the text
            yield return StartCoroutine(FadeTextOut());
            
            // Move to the next message
            currentMessageIndex++;
            
            // Short pause between messages
            yield return new WaitForSeconds(0.5f);
        }
        
        // Fade out to the next scene
        yield return StartCoroutine(FadeToScene());
    }
    
    private IEnumerator FadeTextIn()
    {
        float elapsedTime = 0;
        Color startColor = cutsceneText.color;
        Color targetColor = new Color(1, 1, 1, 1);
        
        while (elapsedTime < textFadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / textFadeInDuration);
            cutsceneText.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
    }
    
    private IEnumerator FadeTextOut()
    {
        float elapsedTime = 0;
        Color startColor = cutsceneText.color;
        Color targetColor = new Color(1, 1, 1, 0);
        
        while (elapsedTime < textFadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / textFadeOutDuration);
            cutsceneText.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
    }
    
    private IEnumerator FadeToScene()
    {
        // Final fade out to transition to the next scene
        yield return new WaitForSeconds(0.5f);
        LoadTargetScene();
    }
    
    private void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            // Use the first stage from GameManager if available
            GameManager gameManager = Object.FindAnyObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.AdvanceToNextStage();
            }
            else
            {
                Debug.LogError("No target scene specified and no GameManager found!");
            }
        }
    }
    
    private void SkipToTargetScene()
    {
        LoadTargetScene();
    }
    
    public void SkipCutscene()
    {
        // Used to skip the cutscene when player presses a key
        StopAllCoroutines();
        StartCoroutine(FadeToScene());
    }
    
    private void Update()
    {
        // Allow skipping the cutscene with any key
        if (Input.anyKeyDown)
        {
            SkipCutscene();
        }
    }
}