using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
	[Header("Stage Settings")]
	[SerializeField] private string mainStageName;
	
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
}
