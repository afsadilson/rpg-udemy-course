using UnityEngine;

public class EnemyState
{

    protected EnemyStateMachine enemyStateMachine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;

    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName) {
        enemyBase = _enemyBase;
        enemyStateMachine = _enemyStateMachine;
        animBoolName = _animBoolName;
    }

    public virtual void Enter() {
        triggerCalled = false;
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Update() {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit() {
        enemyBase.anim.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimName(animBoolName);
    }

    public virtual void AnimationFinishTrigger() {
        triggerCalled = true;
    }
}
