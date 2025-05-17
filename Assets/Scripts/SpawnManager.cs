using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform defaultSpawnPoint;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            float x = PlayerPrefs.GetFloat("SpawnX", 0);
            float y = PlayerPrefs.GetFloat("SpawnY", 0);
            float z = PlayerPrefs.GetFloat("SpawnZ", 0);

            player.transform.position = new Vector3(x,y,z);

            PlayerPrefs.DeleteKey("UseCustomSpawn");
        }
        else if (defaultSpawnPoint != null)
        {
            player.transform.position = defaultSpawnPoint.position;
        }
    }
}
