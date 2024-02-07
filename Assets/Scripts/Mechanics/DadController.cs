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
    public class DadController : KinematicObject
    {
        #region Inspector View
        [Header("Speed")]
        [Range(1, 10f)]
        public float maxSpeed = 7;

        #endregion

        private PlayerController child;
        Vector2 move;
        internal BoxCollider2D collider2d;
        internal SpriteRenderer spriteRenderer;

        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        void Awake()
        {
            collider2d = GetComponent<BoxCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            child = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        protected override void Start()
        {
            move = child.isFacingRight ? Vector2.right : -Vector2.right;
            spriteRenderer.flipX = child.spriteRenderer.flipX;
        }

        protected override void Update()
        {
            ComputeVelocity();
            base.Update();
        }

        protected override void ComputeVelocity()
        {
            // Set velocity to move the character continuously forward
            targetVelocity = move * maxSpeed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
                Schedule<EnemyDeath>().enemy = enemy;
                Destroy(collision.gameObject, 1f);

                // Make the dad leave the scene by sliding away
            }
        }
    }
}