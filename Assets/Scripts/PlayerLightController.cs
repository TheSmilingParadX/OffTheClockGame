using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLightController : MonoBehaviour
{
    [Header("---------- Light Settings ----------")]
    [SerializeField] private Light2D playerLight;

    [Header("---------- Flicker Settings ----------")]
    [SerializeField] private float normalIntensity = 1.0f;
    [SerializeField] private float flickerLowIntensity = 0.15f;
    [SerializeField] private float firstFlickerInterval = 3.0f;
    [SerializeField] private float secondFlickerInterval = 1.0f;

    private AnomalyController anomalyController;
    private int prevRemainingInteractions;
    private bool isFlickering = false;
    private Coroutine flickerCoroutine;

    void Start()
    {
        if (playerLight == null)
        {
            playerLight = GetComponentInChildren<Light2D>();

            if (playerLight == null && transform.parent != null)
            {
                playerLight = transform.parent.GetComponentInChildren<Light2D>();
            }
            if (playerLight == null)
            {
                Debug.LogError("Player Light not found! Please assign a Light2D component in the inspector");
            }
        }

        anomalyController = Object.FindFirstObjectByType<AnomalyController>();

        if (anomalyController != null)
        {
            prevRemainingInteractions = anomalyController.GetRemainingInteractions();
        }
        else
        {
            Debug.LogError("AnomalyController not found in the scene!");
        }
    }

    void Update()
    {
        if (anomalyController == null) return;

        int currInteractions = anomalyController.GetRemainingInteractions();

        if (currInteractions < prevRemainingInteractions)
        {
            if (!anomalyController.IsAnomalyFound() && anomalyController.HasAnomaly())
            {
                HandleWrongInteraction(currInteractions);
            }

            prevRemainingInteractions = currInteractions;
        }

        if (anomalyController.IsAnomalyFound() && isFlickering)
        {
            StopFlickering();
            RestoreLight();
        }
    }

    void HandleWrongInteraction(int remainingInteractions)
    {
        StopFlickering();

        switch (remainingInteractions)
        {
            case 2:
                Debug.Log("First wrong interaction - slow flicker");
                flickerCoroutine = StartCoroutine(FlickerLight(firstFlickerInterval));
                break;

            case 1:
                Debug.Log("Second wrong interaction - fast flicker");
                flickerCoroutine = StartCoroutine(FlickerLight(secondFlickerInterval));
                break;

            case 0:
                Debug.Log("Third wrong interaction - light off and reset");
                TurnOffLight();
                if (!anomalyController.IsAnomalyFound())
                {
                    StartCoroutine(ResetAfterDelay(1.5f));
                }
                else
                {
                    StartCoroutine(RestoreLightAfterDelay(1.0f));
                }
                break;
        }
    }

    IEnumerator FlickerLight(float interval)
    {
        isFlickering = true;

        while (true)
        {
            playerLight.intensity = flickerLowIntensity;
            yield return new WaitForSeconds(interval * 0.3f);

            playerLight.intensity = normalIntensity;
            yield return new WaitForSeconds(interval * 0.7f);
        }
    }

    void StopFlickering()
    {
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
            isFlickering = false;
        }
    }

    void RestoreLight()
    {
        playerLight.intensity = normalIntensity;
    }

    void TurnOffLight()
    {
        StopFlickering();
        playerLight.intensity = 0f;
    }

    IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetToStageOne();
        }
    }

    IEnumerator RestoreLightAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RestoreLight();
    }
}
