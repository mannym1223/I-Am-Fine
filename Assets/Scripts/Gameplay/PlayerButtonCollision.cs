using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when a player collides with a button.
    /// </summary>
    /// <typeparam name="PlayerCollision"></typeparam>
    public class PlayerButtonCollision : Simulation.Event<PlayerButtonCollision>
    {
        public PlayerController player;
        public ButtonInstance button;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            AudioSource.PlayClipAtPoint(button.tokenCollectAudio, button.transform.position);
            
            //TODO: add shake effect when triggered
        }
    }
}