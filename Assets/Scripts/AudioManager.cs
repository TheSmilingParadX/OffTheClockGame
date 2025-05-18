using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource ambienceSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip footsteps;
    public AudioClip ambience;

    private void Start()
    {
        ambienceSource.clip = ambience;
        ambienceSource.Play();
    }
}
