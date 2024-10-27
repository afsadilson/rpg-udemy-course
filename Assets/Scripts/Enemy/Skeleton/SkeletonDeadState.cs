using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{

    private Enemy_Skeleton enemy;

    public SkeletonDeadState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter() {
        base.Enter();

        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = .15f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.linearVelocity = new Vector2(0, 10);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
