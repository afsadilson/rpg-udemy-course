using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack Info")]
    public bool isBusy { get; private set; }
    public Vector2[] attackMoviments;
    public float counterAttackDuration = .2f;
    public float swordReturnImpact;

    [Header("Move Info")]
    public float moveSpeed = 7f;
    public float jumpForce;

    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }

    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; } // throwing sword

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackholeState blackholeState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackholeState = new PlayerBlackholeState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Dead");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);

        skill = SkillManager.instance;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.F))
            skill.crystal.CanUseSkill();
    }

    public void AssignNewSword(GameObject _newSword) {
        sword = _newSword;
    }

    public void CatchTheSword() {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

    public virtual void AnimationTrigger() {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    public IEnumerator BusyFor(float _seconds) {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    private void CheckForDashInput() {

        if (Input.GetKeyDown(KeyCode.LeftShift) && skill.dash.CanUseSkill() && !IsWallDetected()) {

            dashDir = Input.GetAxisRaw("Horizontal");
            
            if (dashDir == 0)
                dashDir = facingDir;
            
            stateMachine.ChangeState(dashState);
        }
    }

    public override void Dead()
    {
        base.Dead();

        stateMachine.ChangeState(deadState);
    }


}
