using Platformer.Gameplay;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Used to move a transform relative to the main camera position with a scale factor applied.
    /// This is used to implement parallax scrolling effects on different branches of gameobjects.
    /// </summary>
    public class TrapTileMap : MonoBehaviour
    {
        public AudioClip tileHitAudio;

        protected TilemapCollider2D tileCollider;

        void Awake()
        {
            tileCollider = GetComponent<TilemapCollider2D>();
        }

		private void OnTriggerStay2D(Collider2D collision)
		{
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
			if (player != null)
			{
				PlayerStayed(player);
			}
		}

        private void PlayerStayed(PlayerController player)
        {
			var ev = Schedule<PlayerHurt>();
			ev.hurtAudio = tileHitAudio;
            ev.bounceVector = Vector2.zero;
			ev.player = player;
		}

	}
}