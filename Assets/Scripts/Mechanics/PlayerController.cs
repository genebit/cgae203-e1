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
    public class PlayerController : KinematicObject
    {
        #region Inspector View
        [Header("Skills")]
        public TextMeshProUGUI promptText;
        public GameObject bulletPrefab;

        [Header("Ultimate Skill")]
        public GameObject dadPrefab;
        public TextMeshProUGUI ultTextCallout;
        public ParticleSystem ultSpeedLineParticle;
        public Transform firePoint;

        public RaycastHit2D hit;

        public ParticleSystem jumpParticle;

        [Header("Speed")]
        [Range(1, 10f)]
        public float maxSpeed = 7;
        [Range(1, 10f)]
        public float jumpTakeOffSpeed = 7;

        [Header("Controls")]
        public bool isFacingRight;
        public JumpState jumpState = JumpState.Grounded;
        public bool controlEnabled = true;

        [Header("Audio Clips")]
        public AudioClip jumpAudio;
        public AudioClip landedAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        #endregion
        
        private bool stopJump;
        internal BoxCollider2D collider2d;
        internal AudioSource audioSource;
        internal Health health;
        internal Animator animator;
        
        bool jump;
        Vector2 move;
        internal SpriteRenderer spriteRenderer;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<BoxCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = Input.GetAxis("Horizontal");

                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
            }
            else
            {
                move.x = 0;
            }

            UpdateJumpState();
            base.Update();
            isFacingRight = spriteRenderer.flipX == false;

            Schedule<PlayerSkill>().player = this;
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;

                    jumpParticle.Play();
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;

                    jumpParticle.Play();
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y *= model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
            {
                spriteRenderer.flipX = false;
                jumpParticle.Play();
            }
            else if (move.x < -0.01f)
            {
                spriteRenderer.flipX = true;
                jumpParticle.Play();
            }

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);


            targetVelocity = move * maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}