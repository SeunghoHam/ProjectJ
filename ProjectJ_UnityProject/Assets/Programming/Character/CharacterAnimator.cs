using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;
    private float _walkValue;
    private bool _isMove;
    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
    }
/*
    public bool IsMove
    {
        get { return _isMove; }
        set
        {
            if (_isMove != value)
            {
                _isMove = value;
                Debug.Log("IsMove Value : " + value);
                _animator.SetBool("isMove", _isMove);
            }
        }
    }*/
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
    /*
    public void Anim_IsMove(bool value)
    {
        if (value)
            _animator.SetBool("isMove", true);
        else
            _animator.SetBool("isMove", false);
    }*/
}
