using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    private int comboCounter;
    private float lastTimeAttacked;
    private float comboWindow = 2;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = .1f;

        if (comboCounter > 2 || Time.time > lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);

        xInput = Input.GetAxisRaw("Horizontal");
        float attackDir = xInput != 0 ? xInput : player.facingDir;
        player.SetVelocity(player.attackMoviments[comboCounter].x * attackDir, player.attackMoviments[comboCounter].y);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled == true)
            stateMachine.ChangeState(player.idleState);

        if (stateTimer < 0)
            player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .15f);

        comboCounter++;
        lastTimeAttacked = Time.time;
    }
}
