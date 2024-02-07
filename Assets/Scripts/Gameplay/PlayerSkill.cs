using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using UnityEngine.UI;

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
            
            // NOTE(Gene): What if there's a gap. the ultimate should only be done if the distance has no gaps.
            // NOTE(Gene): Add logic here that if there's enough points to do this.

            if (Input.GetKeyDown(KeyCode.L))
            {
                PerformUltimate();
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
            GameObject.Instantiate(player.dadPrefab, dadSpawnPosition, Quaternion.identity);

            CostPoints(1000);
        }

        void ShootBullet()
        {
            GameObject.Instantiate(player.bulletPrefab, player.firePoint.position, player.firePoint.rotation);
            player.animator.SetTrigger("attack");

            CostPoints(100);
        }

        void CostPoints(int points)
        {
            model.pointsSlider.value -= points;
        }
    }
}