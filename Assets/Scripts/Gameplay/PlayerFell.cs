using Platformer.Core;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;

public class PlayerFell : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        if (other.CompareTag("Player"))
        {
            var player = model.player;
            player.health.Die();

            Schedule<PlayerDeath>();
        }
    }
}
