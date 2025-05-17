using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private GameObject currentTeleporter;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S is pressed.");
            if (currentTeleporter != null)
            {
                Debug.Log("Teleporting...");
                transform.position = currentTeleporter.GetComponent<Teleporter>().GetDestination().position;
            }
            else
            {
                Debug.Log("No teleporter in range.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            currentTeleporter = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            if (collision.gameObject == currentTeleporter)
            {
                currentTeleporter = null;
            }
        }
    }
}
