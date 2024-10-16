using System;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Stunned info")]
    public float stunningDuration;
    public Vector2 stunningDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    
    [Header("Attack info")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;
    
    
    public EnemyStateMachine enemyStateMachine { get; private set; }

    protected override void Awake() {
        base.Awake();

        enemyStateMachine = new EnemyStateMachine();
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();
        
        enemyStateMachine.currentState.Update();
    }

    public virtual void OpenCounterAttackWindow() {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow() {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned() {
        if (canBeStunned) {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 5, whatIsPlayer);

    public virtual void AnimationFinishTrigger() => enemyStateMachine.currentState.AnimationFinishTrigger();

    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + 5 * facingDir, wallCheck.position.y));
    }
}
