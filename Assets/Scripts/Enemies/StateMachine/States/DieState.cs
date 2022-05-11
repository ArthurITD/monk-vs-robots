using Opsive.Shared.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : BaseState
{
    private void OnEnable()
    {
        //Play death animation
        EventHandler.ExecuteEvent(stateMachine.spawnerRoot,"RobotDied", stateMachine);
        EventHandler.ExecuteEvent(WavesManager.Instance, "RobotDied");
        stateMachine.ResetStateMachine();
        this.enabled = false;
    }
}
