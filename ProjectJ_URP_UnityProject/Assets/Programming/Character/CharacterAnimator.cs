using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;
    private float _walkValue;

    // �����⵿���� input�� ������ �ȵǰ�, �������϶��� �̵��ӵ� ���Ҹ� �ؾߵǴϱ� �ٸ� ������ �Ҵ���
    private bool _isRolling;
    private bool _isJumping;
    

    public enum ChaAnimState
    { 
        Idle, // ���� ���� ����
        Doing, // �Ӷ� ����������
    }
    public ChaAnimState AnimState;

    public bool IsRolling
    {
        get
        {
            return _isRolling;
        }
        set
        {
            if (_isRolling != value)
            {
                //Debug.Log("IsDoing ���� : " + value);
                _isRolling = value;
            }
        }
    }
    public bool IsJumping
    {
        get {return _isJumping;}
        set
        {
            if (_isJumping != value)
                _isJumping = value;
        }
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
            if(_walkValue != value)
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

}
