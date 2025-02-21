using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        /// <summary>
        /// Invincibility time when player is hurt
        /// </summary>
        public float iFrames = 3f;

        /// <summary>
        /// Time between player sprite flashing when hurt
        /// </summary>
        public float flashDelay;

        /// <summary>
        /// Layers to ignore collision with when player is invincible
        /// </summary>
        public LayerMask ignoreWhileInvincible;
        private int ignoreMaskIntValue;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            ignoreMaskIntValue = (int)Mathf.Log(ignoreWhileInvincible.value, 2);
        }

        public void TookDamage()
        {
			UpdateLookBasedOnHealth();
			MakeInvincible();
            FlashPlayer();
        }

        protected void FlashPlayer()
        {
            StartCoroutine(BeginPlayerFlashing());
        }

        protected void MakeInvincible()
        {
			collider2d.excludeLayers |= 1 << ignoreMaskIntValue;
			StartCoroutine(BeginInvincibleCountdown());
        }

        protected void UpdateLookBasedOnHealth()
        {
            Color color = spriteRenderer.color;
            float healthRatio = (float)health.CurrentHP / (float)health.maxHP;
            color.r = 1 - healthRatio;
			color.g = healthRatio;
			color.b = healthRatio;

			spriteRenderer.color = color;
		}

        private IEnumerator BeginInvincibleCountdown()
        {
            yield return BeginCountdown(iFrames);
			collider2d.excludeLayers &= ~(1 << ignoreMaskIntValue);
			yield return null; 
        }

        private IEnumerator BeginPlayerFlashing()
        {
			float count = 0f;
            float flashTime = 0f;
            Color color = spriteRenderer.color;
			while (count < iFrames)
			{
				yield return new WaitForFixedUpdate();
				count += Time.deltaTime;
                flashTime += Time.deltaTime;
                if (flashTime > flashDelay)
                {
                    // alternate alpha
                    color.a = color.a == 1f ? 0.5f : 1f;
                    spriteRenderer.color = color;
                    flashTime = 0f;
                }
			}
            // make sure color is reset
            color.a = 1f;
            spriteRenderer.color = color;
		}

        private IEnumerator BeginCountdown(float timeToWait)
        {
			float count = 0f;
			while (count < timeToWait)
			{
				yield return new WaitForFixedUpdate();
				count += Time.deltaTime;
			}
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
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

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