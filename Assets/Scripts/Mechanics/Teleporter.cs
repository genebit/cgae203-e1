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
        teleportOffset = Vector3.zero;
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

        // Store initial position and destination position
        Vector3 initialPosition = player.position;
        Vector3 targetPosition = destination.position + teleportOffset;

        // Disable renderer to make the player invisible
        Renderer playerRenderer = player.GetComponent<Renderer>();
        if (playerRenderer != null)
        {
            playerRenderer.enabled = false;
        }

        // Disable box collider during teleportation
        BoxCollider2D playerCollider = player.GetComponent<BoxCollider2D>();
        playerCollider.isTrigger = true;

        float elapsedTime = 0f;

        // Duration of interpolation
        float duration = 0.35f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate interpolation ratio
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Interpolate position
            player.position = Vector3.Lerp(initialPosition, targetPosition, t);

            yield return null;
        }

        // Ensure player reaches the exact destination
        player.position = targetPosition;

        // Enable renderer back after teleportation
        playerRenderer.enabled = true;

        // Enable box collider back after teleportation
        playerCollider.isTrigger = false;

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