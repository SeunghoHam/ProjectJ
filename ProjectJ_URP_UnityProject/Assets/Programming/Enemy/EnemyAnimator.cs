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

    public void Anim_Jump()
    {
        _animator.SetTrigger("Jump");
    }
    public void Anim_Death()
    {
        _animator.SetTrigger("Death");
    }
    public void Anim_Slash()
    {
        _animator.SetTrigger("Attack");
    }
}
