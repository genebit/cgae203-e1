using Platformer.Gameplay;
using Platformer.Mechanics;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    public class BulletController : MonoBehaviour
    {
        [Range(0f, 20f)]
        public float speed = 10;

        private GameObject player;
        private SpriteRenderer playerSpriteRenderer;
        private Vector2 initialBulletDirection;
        internal SpriteRenderer spriteRenderer;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            // Set the initial bullet direction based on the player facing dir.
            initialBulletDirection = playerSpriteRenderer.flipX ? Vector2.left : Vector2.right;
            spriteRenderer.flipX = playerSpriteRenderer.flipX;
        }

        private void Update()
        {
            // Move the bullet horizontally
            transform.Translate(speed * Time.deltaTime * initialBulletDirection);

            // Destroy the bullet if it goes off-screen
            if (!IsVisible())
                Destroy(gameObject);
        }

        private bool IsVisible()
        {
            // Check if any renderer of the object is visible from any camera
            return GetComponent<Renderer>().isVisible;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                EnemyDeath enemyDeathEvent = Schedule<EnemyDeath>();
                enemyDeathEvent.enemy = collision.gameObject.GetComponent<EnemyController>();

                // Destroy the collided object after a 1s delay
                Destroy(collision.gameObject, 1f);

                Destroy(gameObject);
            }
        }
    }
}