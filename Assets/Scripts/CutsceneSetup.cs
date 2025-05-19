using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutsceneSetup : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Canvas cutsceneCanvas;
    [SerializeField] private Image blackBackground;
    [SerializeField] private TextMeshProUGUI cutsceneTextDisplay;
    
    [Header("Cutscene Settings")]
    [SerializeField] private string firstStageName;
    [SerializeField] private string[] cutsceneMessages = new string[]
    {
        "You find yourself in a strange place...",
        "Something feels... off here.",
        "Find the anomalies and escape before it's too late."
    };
    
    void Awake()
    {
        // Ensure we have a canvas
        if (cutsceneCanvas == null)
        {
            cutsceneCanvas = GetComponentInChildren<Canvas>();
            if (cutsceneCanvas == null)
            {
                cutsceneCanvas = CreateCanvas();
            }
        }
        
        // Setup the black background
        if (blackBackground == null)
        {
            blackBackground = CreateBackground();
        }
        
        // Setup the text display
        if (cutsceneTextDisplay == null)
        {
            cutsceneTextDisplay = CreateTextDisplay();
        }
        
        // Add the CutsceneManager
        CutsceneManager manager = gameObject.GetComponent<CutsceneManager>();
        if (manager == null)
        {
            manager = gameObject.AddComponent<CutsceneManager>();
        }
        
        // Configure the manager with our UI elements and settings
        ConfigureCutsceneManager(manager);
    }
    
    private Canvas CreateCanvas()
    {
        GameObject canvasObject = new GameObject("CutsceneCanvas");
        canvasObject.transform.SetParent(transform);
        
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        canvasObject.AddComponent<GraphicRaycaster>();
        
        return canvas;
    }
    
    private Image CreateBackground()
    {
        GameObject backgroundObject = new GameObject("BlackBackground");
        backgroundObject.transform.SetParent(cutsceneCanvas.transform, false);
        
        RectTransform rectTransform = backgroundObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        
        Image image = backgroundObject.AddComponent<Image>();
        image.color = Color.black;
        
        return image;
    }
    
    private TextMeshProUGUI CreateTextDisplay()
    {
        GameObject textObject = new GameObject("CutsceneText");
        textObject.transform.SetParent(cutsceneCanvas.transform, false);
        
        RectTransform rectTransform = textObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.1f, 0.4f);
        rectTransform.anchorMax = new Vector2(0.9f, 0.6f);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = 48;
        text.color = Color.white;
        
        return text;
    }
    
    private void ConfigureCutsceneManager(CutsceneManager manager)
    {
        // Using reflection to set the serialized fields since they're private
        var serializedField = typeof(CutsceneManager).GetField("blackOverlay", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        if (serializedField != null) serializedField.SetValue(manager, blackBackground);
        
        serializedField = typeof(CutsceneManager).GetField("cutsceneText", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        if (serializedField != null) serializedField.SetValue(manager, cutsceneTextDisplay);
        
        serializedField = typeof(CutsceneManager).GetField("targetSceneName", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        if (serializedField != null) serializedField.SetValue(manager, firstStageName);
        
        serializedField = typeof(CutsceneManager).GetField("cutsceneMessages", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        if (serializedField != null)
        {
            System.Collections.Generic.List<string> messages = new System.Collections.Generic.List<string>(cutsceneMessages);
            serializedField.SetValue(manager, messages);
        }
    }
}