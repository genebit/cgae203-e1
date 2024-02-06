using Platformer.Core;
using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the health component on an enemy has a hitpoint value of  0.
    /// </summary>
    /// <typeparam name="EnemyDeath"></typeparam>
    public class EnemyDeath : Simulation.Event<EnemyDeath>
    {
        public EnemyController enemy;

        public override void Execute()
        {
            // Change the layer to let player/enemy collide w/o issue
            enemy.gameObject.layer = LayerMask.NameToLayer("Dead Enemy");
            enemy.control.enabled = false;

            if (enemy._audio && enemy.ouch)
                enemy._audio.PlayOneShot(enemy.ouch);

            enemy.deathParticleSystem.Play();
            enemy.control.GetComponent<Animator>().SetBool("death", true);
        }
    }
}