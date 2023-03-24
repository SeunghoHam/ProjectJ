using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
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



    // PlayerState
    private CharacterAnimator _characterAnimator;
    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
        _characterAnimator = Character.Instance.Animator;
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
    public void Anim_DamagePoint() // �������� �ִ� ����
    {
        if(_enemy.CanHit)
        {
            if (_characterAnimator.IsRolling)
            {
                DebugManager.ins.Log("ȸ�� ����", DebugManager.TextColor.Blue);
            }
            else 
            {
                DebugManager.ins.Log("ȸ�� ����", DebugManager.TextColor.Red);
                Character.Instance.Damaged(1);
            }
        }
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
        // AI ��ƾ �ٽ� �����ϵ���
    }
}
