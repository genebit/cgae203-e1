using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    public Transform destination;

    [Range(0, 5f)]
    public float cooldownTime = 2f;
    
    [Tooltip("This is the direction where the player will be pushed out.")]
    public OffsetDirection direction;

    private Vector3 teleportOffset;
    private bool isOnCooldown = false;

    void Start()
    {
        teleportOffset = new Vector3(0f, 0f, 0);
        switch (direction)
        {
            case OffsetDirection.Left:
                teleportOffset.x = -Vector2.left.x;
                break;
            case OffsetDirection.Right:
                teleportOffset.x = -Vector2.right.x;
                break;
            case OffsetDirection.Top:
                teleportOffset.y = -Vector2.up.y;
                break;
            case OffsetDirection.Bottom:
                teleportOffset.y = -Vector2.down.y;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOnCooldown)
        {
            StartCoroutine(TeleportPlayer(other.transform));
        }
    }

    private IEnumerator TeleportPlayer(Transform player)
    {
        // Start cooldown
        isOnCooldown = true;

        // Move player with offset
        player.position = destination.position + teleportOffset; 

        // Wait for cooldown at this gate
        yield return new WaitForSeconds(cooldownTime);

        // End cooldown
        isOnCooldown = false; 
    }

    public enum OffsetDirection
    {
        Left,
        Right,
        Top,
        Bottom
    }
}