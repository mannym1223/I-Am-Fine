using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;
using UnityEngine.Events;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 
    /// </summary>
    public class ButtonInstance : MonoBehaviour
    {
		public AudioClip tokenCollectAudio;
        public bool pressOnStart; // for easier testing
        public UnityEvent onPressedEvent;
        
        internal Animator animator;
        internal bool pressed = false;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

		private void Start()
		{
            if (pressOnStart)
            {
                pressed = true;
				onPressedEvent?.Invoke();
			}
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

            onPressedEvent?.Invoke();
		}

        
    }
}