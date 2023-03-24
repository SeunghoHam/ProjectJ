using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharacterAnimator;

public class EnemyAnimator : MonoBehaviour
{
    private Enemy _enemy;
    private Animator _animator;
    public Animator animator
    {
        get { return _animator; }
    }

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
    }
    public void GetEnemy(Enemy enemy)
    {
        _enemy = enemy;
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
    public void Anim_DamagePoint() // 데미지를 넣는 순간
    {
        if(_enemy.CanHit)
            Character.Instance.Damaged(1);
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

    public void End_Anim_Slash()
    {
        // AI 루틴 다시 동작하도록
    }
}
