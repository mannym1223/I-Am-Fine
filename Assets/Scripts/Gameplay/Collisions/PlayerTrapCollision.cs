using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when a player enters a secret wall.
    /// </summary>
    /// <typeparam name="PlayerCollision"></typeparam>
    public class PlayerTrapCollision : Simulation.Event<PlayerTrapCollision>
    {
        public PlayerController player;
        public TrapTileMap tileMap;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            player = model.player;
            if (tileMap.tileHitAudio != null)
            {
                AudioSource.PlayClipAtPoint(tileMap.tileHitAudio, player.transform.position);
            }

            player.Bounce(1f);
            player.health.Decrement();
        }
    }
}