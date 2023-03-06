using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;
    private float _walkValue;

    // �����⵿���� input�� ������ �ȵǰ�, �������϶��� �̵��ӵ� ���Ҹ� �ؾߵǴϱ� �ٸ� ������ �Ҵ���
    private bool _isRolling;
    private bool _isJumping;
    private bool _isAttacking; // ������
    private bool _isStateChnaging; // �ִϸ����� ���̾� ������

    public Animator animator
    {
        get { return _animator; }
    }
    public enum ChaAnimState
    {
        Idle, // ���� ���� ����
        Doing, // �Ӷ� ����������
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

    private void Update()
    {
        if (_isStateChnaging)
        {
            if (Character.Instance.weaponState == Character.WeaponState.Normal)
            {
                ValueChange(1, 1);
                ValueChange(2, 0);
            }
            else
            {
                ValueChange(1, 0);
                ValueChange(2, 1);
            }
        }
    }
    public void ValueChange(int index, float destValue) // ���̾� �ε��� , ������
    {
        /*
        destValue = Mathf.SmoothDamp
            (_animator.GetLayerWeight(index),
            destValue, ref refValue, smoothTime * Time.deltaTime);
        */
        _animator.SetLayerWeight(index,
            Mathf.Lerp(_animator.GetLayerWeight(0), destValue, 2f* Time.deltaTime));

        if (Mathf.Approximately( Mathf.Round(_animator.GetLayerWeight(index)), destValue))
        {
            IsStateChnaging = false;
            Debug.Log("������Ʈ ���� ��");
        }
    }
    public void Anim_Move()
    {
        _animator.SetTrigger("Move");
    }
    public void Anim_Move_Direction(float value)
    {
        _animator.SetFloat("Directino", value);
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
        animator.SetFloat("WalkValue",walkValue);

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


    public void End_Anim_Roll() // �ִϸ����� Ŭ���� �Ҵ��Ұ�
    {
        AnimState = ChaAnimState.Idle;
        IsRolling = false;
    }
    public void End_Anim_Jump()
    {
        AnimState = ChaAnimState.Idle;
        IsJumping = false;
    }

    public void End_Anim_Slash()
    {
        AnimState = ChaAnimState.Idle;
        IsAttacking = false;
    }
}
