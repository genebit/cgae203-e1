using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    public class PlayerSkill : Simulation.Event<PlayerSkill>
    {
        public PlayerController player;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.J)) ShootBullet();
            if (Input.GetKeyDown(KeyCode.K)) ShootBullet();


            // Perform 2D box cast to detect enemies with a wider "ray"
            RaycastHit2D hit = Physics2D.BoxCast(player.transform.position, new Vector2(2, 1), 0f, (player.isFacingRight ? Vector2.right : Vector2.left), Mathf.Infinity, LayerMask.GetMask("Enemy"));

            // Draw the wider ray for debugging purposes
            Debug.DrawRay(player.transform.position, (player.isFacingRight ? Vector2.right : Vector2.left) * hit.distance, Color.red);

            // NOTE(Gene): What if there's a gap. the ultimate should only be done if the distance has no gaps.

            // Check if the box cast hits an object with the "Enemy" tag
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                // Enemy detected, handle accordingly
                // NOTE(Gene): Add logic here that if there's enough points to do this.
                if (Input.GetKeyDown(KeyCode.L))
                {
                    PerformUltimate();
                }
            }
        }

        void PerformUltimate()
        {
            //	1. Zoom in Camera
            model.cameraZoom.transitionDuration = 0.3f;
            model.cameraZoom.zoomedOrthoSize = 2f;
            model.cameraZoom.TriggerZoom();
            
            //	2. Show Callout
            player.ultTextCallout.gameObject.SetActive(true);

            //  3. Play the "Speed Lines" particle
            player.ultSpeedLineParticle.gameObject.SetActive(true);

            //  4. Spawn papa on the left/right depending on the facing dir. of the player
            // Determine the edge of the screen based on the facing direction of the player
            Vector3 dadSpawnPosition = player.transform.position;
            dadSpawnPosition.y += 2f;
            dadSpawnPosition.x += (player.isFacingRight) ? -5f : 5f;

            // Spawn papa at the widened edge of the screen
            GameObject instantiatedPapa = GameObject.Instantiate(player.dadPrefab, dadSpawnPosition, Quaternion.identity);
        }

        void ShootBullet()
        {
            GameObject.Instantiate(player.bulletPrefab, player.firePoint.position, player.firePoint.rotation);
            player.animator.SetTrigger("attack");
        }
    }
}