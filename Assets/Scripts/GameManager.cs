using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Stage Settings")]
	[SerializeField] private string mainStageName;
    [SerializeField] private string firstStageName;
    [SerializeField] private List<string> randomStageNames = new List<string>();
    [SerializeField] private int maxStageCount = 5;

    [Header("UI References")]
    [SerializeField] private GameObject stageCounterUI;
    [SerializeField] private TMPro.TextMeshProUGUI stageText;
	[SerializeField] private GameObject menu1;
	[SerializeField] private GameObject menu2;

    private int currentStage = 1;
    private string lastRandomStage = "";
    private bool stageComplete = false;
    private bool anomalyPresent = false;
    private bool anomalyFound = false;
	private bool paused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateStageUI();
        
        SetupStageAnomaly();
    }

    private void Start()
    {
        currentStage = 1;
        UpdateStageUI();
    }
	
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(!paused)
			{
				Pause();
			}
			else
			{
				Unpause();
			}
		}
	}
	
	public void Pause()
	{
		paused = true;
		menu1.SetActive(true);
		menu2.SetActive(false);
		Time.timeScale = 0.0f;
	}
	
	public void Unpause()
	{
		paused = false;
		menu1.SetActive(false);
		menu2.SetActive(false);
		Time.timeScale = 1.0f;
	}
	
	public void OpenSettings()
	{
		menu1.SetActive(false);
		menu2.SetActive(true);
	}
	
	public void CloseSettings()
	{
		menu1.SetActive(true);
		menu2.SetActive(false);
	}
	
	public void GoToMain()
	{
		SceneManager.LoadScene(mainStageName);
	}

    public void AdvanceToNextStage()
    {
        if (!stageComplete)
        {
            if (anomalyPresent && !anomalyFound)
            {
                ResetToStageOne();
                return;
            }
        }

        currentStage++;
        
        if (currentStage > maxStageCount)
        {
            currentStage = 1;
        }

        string nextStageName = GetNextStageName();
        SceneManager.LoadScene(nextStageName);
    }

    public string GetNextStageName()
    {
        if (currentStage == 1)
        {
            return firstStageName;
        }
        else
        {
            if (randomStageNames.Count > 1)
            {
                string newStage;
                do
                {
                    int randomIndex = Random.Range(0, randomStageNames.Count);
                    newStage = randomStageNames[randomIndex];
                } while (newStage == lastRandomStage && randomStageNames.Count > 1);

                lastRandomStage = newStage;
                return newStage;
            }
            else if (randomStageNames.Count == 1)
            {
                return randomStageNames[0];
            }
            else
            {
                return firstStageName;
            }
        }
    }

    public void ResetToStageOne()
    {
        currentStage = 1;
        string nextStageName = GetNextStageName();
        SceneManager.LoadScene(nextStageName);
    }

    private void UpdateStageUI()
    {
        if (stageCounterUI != null && stageText != null)
        {
            stageText.text = "Stage " + currentStage;
        }
        else
        {
            GameObject uiObject = GameObject.FindGameObjectWithTag("StageUI");
            if (uiObject != null)
            {
                stageCounterUI = uiObject;
                stageText = uiObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (stageText != null)
                {
                    stageText.text = "Stage " + currentStage;
                }
            }
        }
		
		Unpause();
    }

    private void SetupStageAnomaly()
    {
        AnomalyController anomalyController = Object.FindFirstObjectByType<AnomalyController>();
        
        if (anomalyController != null)
        {
            anomalyPresent = anomalyController.HasAnomaly();
            anomalyFound = false;
            stageComplete = false;
        }
        else
        {
            anomalyPresent = false;
            anomalyFound = false;
            stageComplete = false;
        }
    }

    public void AnomalyDiscovered()
    {
        anomalyFound = true;
        stageComplete = true;
    }

    public bool IsStageComplete()
    {
        return stageComplete;
    }

    public void SetStageComplete(bool complete)
    {
        stageComplete = complete;
    }

    public int GetCurrentStage()
    {
        return currentStage;
    }
}