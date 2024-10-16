using UnityEngine;

public class Enemy_Skeleton : Enemy
{

    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunnedState stunnedState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new SkeletonIdleState(this, enemyStateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, enemyStateMachine, "Move", this);
        battleState = new SkeletonBattleState(this, enemyStateMachine, "Move", this);
        attackState = new SkeletonAttackState(this, enemyStateMachine, "Attack", this);
        stunnedState = new SkeletonStunnedState(this, enemyStateMachine, "Stunned", this);
    }


    protected override void Start()
    {
        base.Start();
        enemyStateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStunned() {
        if (base.CanBeStunned()) {
            enemyStateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }
}
