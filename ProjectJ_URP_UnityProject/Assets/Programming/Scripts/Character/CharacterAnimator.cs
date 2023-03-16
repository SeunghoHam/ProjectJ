using Assets.Scripts.Manager;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;
    private float _walkValue;

    // 구르기동안은 input을 받으면 안되고, 점프중일때는 이동속도 감소만 해야되니까 다른 변수로 할당함
    private bool _isRolling;
    private bool _isJumping;
    private bool _isAttacking; // 공격중(구르기 가능)
    private bool _isStateChnaging; // 애니메이터 레이어 변경중

    private bool _canAttack =true;
    public bool CanAttack
    {
        get { return _canAttack; }
        set 
        {
            if (_canAttack != value)
                _canAttack = value;
        }
    }


    public Animator animator
    {
        get { return _animator; }
    }

    // 이동 및 다른 모션 가능한 거 ->  MultiMoveAnim (걷기랑 겹치면 안되긴함)
    // 불가 -> OnlyThisAnim
    public enum ChaAnimState
    {
        Idle, // 동작 가능 상태
        Jump, // 점프, 
        Roll, //  구르기
        Attack, // 공격중
        SeriesAttackReady, // 연속공격 준비
    }

    public ChaAnimState AnimState = ChaAnimState.Idle;

    #region ::: 애니스테이트 편하게 가져오기 위해서 미리 bool 함수로 정의해두기


    /// <summary> 점프나 아무것도 안할때만 이동되도록함 </summary>
    /// <returns>Idle, Jump </returns>
    public bool ReturnCanMove()
    {
        return AnimState == ChaAnimState.Idle || AnimState == ChaAnimState.Jump;
    }


    /// <summary> 점프중이 아니거나 아무것도 안할때만 점프가능 </summary>
    /// <returns></returns>
    public bool ReturnCanJump()
    {
        return AnimState == ChaAnimState.Idle || 
            AnimState != ChaAnimState.Jump ||
            AnimState != ChaAnimState.Roll;
    }
    #endregion
    public bool IsRolling
    {
        get { return _isRolling; }
        set { if (_isRolling != value) _isRolling = value; }
    }
    public bool IsJumping
    {
        get { return _isJumping; }
        set
        { if (_isJumping != value) _isJumping = value; }
    }
    public bool IsAttacking
    {
        get { return _isAttacking; }
        set
        { if (_isAttacking != value) _isAttacking = value; }
    }
    public bool IsStateChnaging
    {
        get { return _isStateChnaging; }
        set
        { if (_isStateChnaging != value) _isStateChnaging = value; }
    }
    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
    }
    public float WalkValue
    {
        get { return _walkValue; }
        set
        {
            if (_walkValue != value)
            {
                //Debug.Log("WalkValue : " + value);
                _walkValue = value;
                _animator.SetFloat("WalkBlend", _walkValue);
            }
        }
    }

    public void Anim_Move()
    {
        _animator.SetTrigger("Move");
    }
    public void Anim_Roll()
    {
        _animator.SetTrigger("Roll");
    }

    public void Anim_Jump()
    {
        _animator.SetTrigger("Jump");
    }
    public void Anim_Idle()
    {
        _animator.SetTrigger("Idle");
    }

    public void Anim_GetDirection(float dirx, float diry)
    {
        animator.SetFloat("DirX", dirx);
        animator.SetFloat("DirY", diry);
    }
    public void Anim_WalkValue(float walkValue)
    {
        animator.SetFloat("WalkValue", walkValue);
    }

    #region ##### Sword #####
    public void Anim_Sword_Slash1()
    {
        _animator.SetTrigger("Slash1");
    }
    public void Anim_Sword_Slash2()
    {
        _animator.SetTrigger("Slash2");
    }
    public void Anim_DamagePoint() // 데미지를 넣는 순간
    {
        
        if (BattleManager.GetEnemy().Count > 0)
        {
            BattleManager.Attack();
            //Character.Instance.Attack();
        }
        Anim_SeriesAttack_Active();
    }
    public void Anim_SeriesAttack_Active() // 연속공격 활성화
    {
        if(AnimState != ChaAnimState.Attack)
        {
            Debug.Log("다시 공격가능");
            CanAttack = true; // 다시 공격 가능하게 해줘야함
        }
        else // 두번째 타격에서는 공격이 다시 활성화되면 안되니까
        { return; }
        
    }
    public void Anim_SeriesAttack_DeActive() // 연속공격 비활성화 ( EndSlash 에서 이역할을 해도 되지 않을까?)
    {

    }

    public void Anim_Sword_Death1()
    {
        _animator.SetTrigger("Death1");
    }
    public void Anim_Sword_Death2()
    {
        _animator.SetTrigger("Death2");
    }
    public void Anim_Sword_()
    {
        
    }
    #endregion


    public void End_Anim_Roll() // 애니메이터 클립에 할당할거
    {
        IsRolling = false;
        AnimState = ChaAnimState.Idle;
    }
    public void End_Anim_Jump() // 완전한 애니메이션 종료를 의미
    {
        AnimState = ChaAnimState.Idle;
    }
    public void Jump_GravityActive() // 중력 적용 시작 
    {
        IsJumping = false;
    }

    public void End_Anim_Slash()
    {
        //IsAttacking = false;
        AnimState = ChaAnimState.Idle;
        CanAttack = true;
        //DebugManager.ins.Log("EndSlash");
    }
}
