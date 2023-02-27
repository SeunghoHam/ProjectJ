using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;
    private float _walkValue;
    private bool _isDoing;

    public bool IsDoing
    {
        get
        {
            return _isDoing;
        }
        set
        {
            if (_isDoing != value)
            {
                Debug.Log("IsDoing 변경 : " + value);
                _isDoing = value;
            }
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
                Debug.Log("WalkValue : " + value);
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
        IsDoing = false;
    }
}
