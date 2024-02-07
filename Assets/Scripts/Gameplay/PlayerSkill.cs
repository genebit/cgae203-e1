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
            if (Input.GetKeyDown(KeyCode.L)) ShootBullet();


            // Perform 2D box cast to detect enemies with a wider "ray"
            RaycastHit2D hit = Physics2D.BoxCast(player.transform.position, new Vector2(2, 1), 0f, (player.spriteRenderer.flipX ? Vector2.left : Vector2.right), Mathf.Infinity, LayerMask.GetMask("Enemy"));

            // Draw the wider ray for debugging purposes
            Debug.DrawRay(player.transform.position, (player.spriteRenderer.flipX ? Vector2.left : Vector2.right) * hit.distance, Color.red);

            // NOTE(Gene): What if there's a gap. the ultimate should only be done if the distance has no gaps.

            // Check if the box cast hits an object with the "Enemy" tag
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                // Enemy detected, handle accordingly
                if (Input.GetKeyDown(KeyCode.L))
                {
                    model.cameraZoom.transitionDuration = 0.3f;
                    model.cameraZoom.zoomedOrthoSize = 2f;
                    model.cameraZoom.TriggerZoom();
                }
            }
        }

        void ShootBullet()
        {
            GameObject.Instantiate(player.bulletPrefab, player.firePoint.position, player.firePoint.rotation);
            player.animator.SetTrigger("attack");
        }
    }
}