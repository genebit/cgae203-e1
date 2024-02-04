using Platformer.Core;
using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Gameplay
{
    public class PlayerSkill : Simulation.Event<PlayerSkill>
    {
        public PlayerController player;

        public override void Execute()
        {
            if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.J)) ShootBullet();
            if (Input.GetKeyDown(KeyCode.K)) ShootBullet();
            if (Input.GetKeyDown(KeyCode.L)) ShootBullet();
        }

        void ShootBullet()
        {
            GameObject.Instantiate(player.bulletPrefab, player.firePoint.position, player.firePoint.rotation);
            player.animator.SetTrigger("attack");
        }
    }
}