using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Base class for platform types
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Platform : MonoBehaviour
    {
        [HideInInspector]
        public SpriteRenderer spriteRenderer;
		[HideInInspector]
		public Animator animator;
		[HideInInspector]
		public Collider2D collider2d;
        public bool isActive;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            collider2d = GetComponent<Collider2D>();
        }

        public virtual void ActivatePlatform() { }

        public virtual void DeactivatePlatform() { }
    }
}