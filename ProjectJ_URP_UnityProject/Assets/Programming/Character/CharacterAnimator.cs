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
    

    public enum ChaAnimState
    { 
        Idle, // 동작 가능 상태
        Doing, // 머라도 동작이잇음
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
                //Debug.Log("IsDoing 변경 : " + value);
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


    public void End_Anim_Roll() // 애니메이터 클립에 할당할거
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
