using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Movable platform type
    /// </summary>
    public class HiddenPlatform : Platform
    {
        public bool activeOnStart = false;

		private void Start()
		{
			Color color = spriteRenderer.color;
            
            // for easier testing
#if UNITY_EDITOR
            color.a = 1f;
#else
            color.a = 0f;
#endif
            
            spriteRenderer.color = color;
			if (activeOnStart)
            {
                ActivatePlatform();
            }
            else
            {
                DeactivatePlatform();
            }
		}

        public override void ActivatePlatform()
        {
            if(isActive)
            {
                return;
            }

            collider2d.enabled = true;
            isActive = true;
        }

        public override void DeactivatePlatform()
        {
			collider2d.enabled = false;
			isActive = false;
        }
	}
}