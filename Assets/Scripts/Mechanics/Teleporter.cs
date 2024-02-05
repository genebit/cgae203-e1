using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    public Transform destination;

    [Range(0, 5f)]
    public float cooldownTime = 2f;

    private Vector3 teleportOffset = new Vector3(0f, 0f, 0);

    private bool isOnCooldown = false;
    public enum OffsetDirection
    {
        Left,
        Right,
        Top,
        Bottom
    }

    public OffsetDirection direction;

    void Start()
    {
        switch (direction)
        {
            case OffsetDirection.Left:
                teleportOffset.x = Vector2.left.x;
                break;
            case OffsetDirection.Right:
                teleportOffset.x = Vector2.right.x;
                break;
            case OffsetDirection.Top:
                teleportOffset.x = Vector2.up.y;
                break;
            case OffsetDirection.Bottom:
                teleportOffset.x = Vector2.down.y;
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
        isOnCooldown = true; // Start cooldown
        player.position = destination.position + teleportOffset; // Move player with offset

        // Wait for cooldown at this gate
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false; // End cooldown   
    }
}