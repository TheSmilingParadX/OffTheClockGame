using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    private GameObject player;

    public float horiOffset = 0f;
    public float vertOffset = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && spawnPoint != null)
        {
            RespawnPlayer();
        }
    }

    public void RespawnPlayer()
    {
        if (player == null || spawnPoint == null) return;

        SpriteRenderer doorSprite = spawnPoint.GetComponent<SpriteRenderer>();
        SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();

        if (doorSprite != null && playerSprite != null)
        {
            Vector3 doorBottomCenter = spawnPoint.position - new Vector3(0, doorSprite.bounds.extents.y, 0);
            
            Vector3 newPos = doorBottomCenter + new Vector3(horiOffset, vertOffset, -1f);

            player.transform.position = newPos;
        }
        else
        {
            player.transform.position = new Vector3(spawnPoint.position.x + horiOffset, spawnPoint.position.y + vertOffset, spawnPoint.position.z - 1f);
        }
    }
}
