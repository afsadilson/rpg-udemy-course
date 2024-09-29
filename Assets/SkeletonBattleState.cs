using Unity.VisualScripting;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{

    private Enemy_Skeleton enemy;
    private Transform player;
    private int moveDir;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player").transform;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected()) {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance <= enemy.attackDistance) {
                if (CanAttack())
                    enemyStateMachine.ChangeState(enemy.attackState);
            }
        } else {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
                enemyStateMachine.ChangeState(enemy.idleState);
        }

        if (player.position.x > enemy.transform.position.x) {
            moveDir = 1;
        } else if (player.position.x < enemy.transform.position.x) {
            moveDir = -1;
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public bool CanAttack() {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown) {
            enemy.lastTimeAttacked = Time.time;
            
            return true;
        }

        return false;
    }

}
