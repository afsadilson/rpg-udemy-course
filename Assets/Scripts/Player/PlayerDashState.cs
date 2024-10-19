using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.dashDuration;

        player.skill.clone.CreateClone(player.transform);
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundedDetected() && player.IsWallDetected()) {
            stateMachine.ChangeState(player.wallSlideState);
            return;
        }
        
        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, rb.linearVelocity.y);
    }

}
