using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;
    private float _walkValue;

    // 구르기동안은 input을 받으면 안되고, 점프중일때는 이동속도 감소만 해야되니까 다른 변수로 할당함
    private bool _isRolling;
    private bool _isJumping;
    private bool _isAttacking; // 공격중
    private bool _isStateChnaging; // 애니메이터 레이어 변경중

    public Animator animator
    {
        get { return _animator; }
    }

    // 이동 및 다른 모션 가능한 거 ->  MultiMoveAnim (걷기랑 겹치면 안되긴함)
    // 불가 -> OnlyThisAnim
    public enum ChaAnimState
    {
        Idle, // 동작 가능 상태
        MultiAnim, // 점프, 
        OnlyThisAnim, // 공격, 구르기
    }

    public ChaAnimState AnimState;

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
    public void Anim_Sword_Slash()
    {
        _animator.SetTrigger("Slash");
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
    public void End_Anim_Jump()
    {
        IsJumping = false;
        AnimState = ChaAnimState.Idle;
    }

    public void End_Anim_Slash()
    {
        IsAttacking = false;
        AnimState = ChaAnimState.Idle;
        DebugManager.ins.Log("EndSlash");
    }
}
