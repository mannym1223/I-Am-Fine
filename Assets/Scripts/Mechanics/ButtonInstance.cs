using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Marks a trigger as a VictoryZone, usually used to end the current game level.
    /// </summary>
    public class ButtonInstance : MonoBehaviour
    {
		public AudioClip tokenCollectAudio;
        
        internal Animator animator;
        internal bool pressed = false;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

		void OnTriggerEnter2D(Collider2D collider)
        {
            var player = collider.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                PlayerEnter(player);
            }
        }

        protected void PlayerEnter(PlayerController player) {
            if (pressed) {
                return;
            }
            pressed = true;

            animator.SetTrigger("Pressed");
			
            var ev = Schedule<PlayerButtonCollision>();
			ev.button = this;
			ev.player = player;
		}
    }
}