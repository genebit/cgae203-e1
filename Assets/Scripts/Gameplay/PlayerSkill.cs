using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.Gameplay
{
    public class PlayerSkill : Simulation.Event<PlayerSkill>
    {
        public PlayerController player;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        private readonly CoroutineManager coroutineManager = CoroutineManager.Instance;

        public override void Execute()
        {
            if (((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.J)) && model.points.value < 100) ||
                (Input.GetKeyDown(KeyCode.K) && model.points.value < 250) ||
                (Input.GetKeyDown(KeyCode.L) && model.points.value < 1000))
            {
                player.promptText.gameObject.SetActive(true);
                coroutineManager.StartCoroutine(DisablePromptAfterDelay(3f));
            }
            else
            {
                if (model.points.value >= 100 && (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.J))) ShootBullet();
                if (model.points.value >= 250 && Input.GetKeyDown(KeyCode.K)) ShootBullet();
                if (model.points.value == 1000 && Input.GetKeyDown(KeyCode.L)) PerformUltimate();
            }

        }

        IEnumerator DisablePromptAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            player.promptText.gameObject.SetActive(false);
        }

        void CostPoints(int points) => model.points.value -= points;

        void PerformUltimate()
        {
            //  0. play animation of crying
            player.animator.SetBool("cry", true);

            // NOTE(Gene): What if there's a gap. the ultimate should only be done if the distance has no gaps.
            //	1. Zoom in Camera
            model.cameraZoom.transitionDuration = 0.3f;
            model.cameraZoom.zoomedOrthoSize = 2f;
            model.cameraZoom.TriggerZoom();

            //	2. Show Callout
            player.ultTextCallout.gameObject.SetActive(true);

            //  3. Play the "Speed Lines" particle
            player.ultSpeedLineParticle.gameObject.SetActive(true);

            //  4. Spawn papa on the left/right depending on the facing dir. of the player
            //  Determine the edge of the screen based on the facing direction of the player
            Vector3 dadSpawnPosition = player.transform.position;
            dadSpawnPosition.y += 2f;

            float positionOffset = 5f;
            dadSpawnPosition.x += (player.isFacingRight) ? -positionOffset : positionOffset;

            //  5. Spawn papa at the widened edge of the screen
            GameObject.Instantiate(player.dadPrefab, dadSpawnPosition, Quaternion.identity);

            CostPoints(1000);
        }

        void ShootBullet()
        {
            GameObject.Instantiate(player.bulletPrefab, player.firePoint.position, player.firePoint.rotation);
            player.animator.SetTrigger("attack");

            CostPoints(100);
        }
    }
}