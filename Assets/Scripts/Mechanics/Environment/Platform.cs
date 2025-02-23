using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Base class for platform types
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Platform : MonoBehaviour
    {

        public bool isActive;
        public bool isOneWay;
        public bool oneWayCollideOnStart;

        [HideInInspector]
        public SpriteRenderer spriteRenderer;
		[HideInInspector]
		public Animator animator;
		[HideInInspector]
		public Collider2D collider2d;

		void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            collider2d = GetComponent<Collider2D>();
            if(isOneWay && !oneWayCollideOnStart)
            {
                collider2d.isTrigger = true;
            }
            else if(oneWayCollideOnStart)
            {
				collider2d.isTrigger = false;
			}
        }

        public virtual void ActivatePlatform() { }

        public virtual void DeactivatePlatform() { }

		private void OnTriggerExit2D(Collider2D collision)
		{
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if(player != null)
            {
                // player will land on top
                if (player.transform.position.y > transform.position.y)
                {
                    collider2d.isTrigger = false;
                }
            }
		}

		private void OnCollisionExit2D(Collision2D collision)
		{
			if(isOneWay)
            {
                collider2d.isTrigger = true;
            }
		}
	}
}