using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player is spawned after dying.
    /// </summary>
    public class PlayerSpawned : Simulation.Event<PlayerSpawned>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;
            player.collider2d.enabled = true;
            player.controlEnabled = false;
			player.jumpState = PlayerController.JumpState.Grounded;

			model.virtualCamera.m_Follow = player.transform;
            model.virtualCamera.m_LookAt = player.transform;

			model.spawnCamera.m_Follow = player.transform;
			model.spawnCamera.m_LookAt = player.transform;

            Simulation.Schedule<EnablePlayerInput>();
		}
    }
}