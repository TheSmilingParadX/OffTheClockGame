using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
	[Header("Stage Settings")]
	[SerializeField] private string mainStageName;
    [SerializeField] private string firstStageName;
	[SerializeField] private string cutsceneStageName = "StageCUTSCENE";
	
	[Header("UI References")]
	[SerializeField] private GameObject menu1;
	[SerializeField] private GameObject menu2;
	
	private bool paused = false;
	
	private void Start()
    {
        Unpause();
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
		Debug.Log("Paused!");
		paused = true;
		menu1.SetActive(true);
		menu2.SetActive(false);
	}
	
	public void Unpause()
	{
		paused = false;
		menu1.SetActive(false);
		menu2.SetActive(false);
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
	
	public void LoadStageOne()
	{
		if (!string.IsNullOrEmpty(cutsceneStageName))
		{
			SceneManager.LoadScene(cutsceneStageName);
		}
		else
		{
			SceneManager.LoadScene(firstStageName);
		}
	}
	
	public void ExitGame()
	{
		Application.Quit();
	}
}
