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
    public class PlayerSecretWallEntered : Simulation.Event<PlayerSecretWallEntered>
    {
        public PlayerController player;
        public SecretWall wall;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            if (wall.wallEnteredAudio != null)
            {
                AudioSource.PlayClipAtPoint(wall.wallEnteredAudio, wall.transform.position);
            }
        }
    }
}