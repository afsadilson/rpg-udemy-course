using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space)){
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

        if (xInput != 0) {
            if (xInput != player.facingDir) {
                stateMachine.ChangeState(player.idleState);
            } else {
                if (yInput < 0) {
                    player.SetVelocity(0, rb.linearVelocity.y);
                } else {
                    player.SetVelocity(0, rb.linearVelocity.y * .7f);
                }
            }
        }

        if (player.IsGroundedDetected())
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
