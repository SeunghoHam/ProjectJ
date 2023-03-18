using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator _animator;
    public Animator animator
    {
        get { return _animator; }
    }

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
    }
    public enum EnemyAnimState 
    { 
        Idle,
        OnlyThisAnim
    }
    public EnemyAnimState AnimState;

    public void Anim_Attack1()
    {
        _animator.SetTrigger("Attack1");
    }
    public void Anim_Attack2()
    {
        _animator.SetTrigger("Attack2");
    }
    public void Anim_Attack3()
    {
        _animator.SetTrigger("Attack3");
    }
    public void Anim_Damaged()
    {
        _animator.SetTrigger("Damaged");
    }
    public void Anim_Jump()
    {
        _animator.SetTrigger("Jump");
    }
    public void Anim_Death()
    {
        _animator.SetTrigger("Death");
    }

    
}
