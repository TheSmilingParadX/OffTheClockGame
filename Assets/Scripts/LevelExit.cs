using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool requiresKeyPress = true;
    [SerializeField] private KeyCode interactKey = KeyCode.S;
    [SerializeField] private float transitionDelay = 0.5f;

    private bool playerInRange = false;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (playerInRange)
        {
            if (!requiresKeyPress || Input.GetKeyDown(interactKey))
            {
                StartCoroutine(TransitionToNextStage());
            }
        }
    }

    private IEnumerator TransitionToNextStage()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        yield return new WaitForSeconds(transitionDelay);

        if (spawnPoint != null)
        {
            PlayerPrefs.SetFloat("SpawnX", spawnPoint.position.x);
            PlayerPrefs.SetFloat("SpawnY", spawnPoint.position.y);
            PlayerPrefs.SetFloat("SpawnZ", spawnPoint.position.z);
            PlayerPrefs.SetInt("UseCustomSpawn", 1);
        }
        else
        {
            PlayerPrefs.SetInt("UseCustomSpawn", 0);
        }

        // Use the GameManager to handle stage progression
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AdvanceToNextStage();
        }
        else
        {
            // Fallback in case GameManager is not available
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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