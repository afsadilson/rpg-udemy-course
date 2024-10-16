using Unity.VisualScripting;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{

    private Enemy_Skeleton enemy;

    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter() {
        base.Enter();
        
        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemy.stunningDuration;
        
        rb.linearVelocity = new Vector2(enemy.stunningDirection.x * -enemy.facingDir, enemy.stunningDirection.y);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            enemyStateMachine.ChangeState(enemy.idleState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.fx.Invoke("CancelRedColorBlink", 0);
    }
}
