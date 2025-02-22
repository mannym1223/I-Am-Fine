using Platformer.Core;
using Platformer.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationGameObject : MonoBehaviour
{
    public void CallEnablePlayerInput()
    {
        Simulation.Schedule<EnablePlayerInput>();
    }
}
