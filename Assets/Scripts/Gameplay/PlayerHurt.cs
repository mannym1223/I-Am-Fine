using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{

    /// <summary>
    /// Fired when a Player takes damage.
    /// </summary>
    /// <typeparam name="PlayerHurt"></typeparam>
    public class PlayerHurt : Simulation.Event<PlayerHurt>
    {
        public EnemyController enemy;
        public PlayerController player;
        public AudioClip hurtAudio;
        public Vector2 bounceVector = new Vector2(1f, 1f);

        PlatformerModel model = GetModel<PlatformerModel>();

        public override void Execute()
        {
			player = model.player;
			if (hurtAudio != null)
			{
				AudioSource.PlayClipAtPoint(hurtAudio, player.transform.position);
			}

			player.Bounce(bounceVector);
			player.health.Decrement();
            player.TookDamage();
		}

    }
}