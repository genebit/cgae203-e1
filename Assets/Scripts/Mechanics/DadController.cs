using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using System.Diagnostics;
using TMPro;

namespace Platformer.Mechanics
{
    public class DadController : MonoBehaviour
    {
        #region Inspector View
        [Header("Speed")]
        [Range(1, 10f)]
        public float maxSpeed = 2;
        #endregion

        private PlayerController child;
        private ParticleSystem dustParticle;
        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            dustParticle = gameObject.GetComponentInChildren<ParticleSystem>();
            child = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

            gameObject.GetComponent<SpriteRenderer>().flipX = child.spriteRenderer.flipX;
            child.controlEnabled = false;
        }

        private void Update()
        {
            dustParticle.Play();

            // Set initial velocity to move forward
            rb.velocity = new Vector2((child.isFacingRight) ? maxSpeed : -maxSpeed, 0);

            if (!GetComponent<Renderer>().isVisible)
            {
                child.controlEnabled = true;

                child.ultTextCallout.gameObject.SetActive(false);
                child.ultSpeedLineParticle.gameObject.SetActive(false);

                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
                Schedule<EnemyDeath>().enemy = enemy;
                Destroy(collision.gameObject, 1f);
            }
        }
    }
}
