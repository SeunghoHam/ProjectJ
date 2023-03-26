using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Enemy _enemy;
    [SerializeField] protected EnemyMovement _movement; // Ư�� �ִϸ��̼� ���ۿ��� �̵���Ű�� ���ؼ� movement �Ҵ�

    protected Animator _animator;
    public Animator animator
    {
        get { return _animator; }
    }

    public enum EnemyAnimState
    {
        Idle,
        OnlyThisAnim
    }
    public EnemyAnimState AnimState;


    // ĳ������ ȸ�� ���� ��ȯ�� ���� �Ҵ�
    protected CharacterAnimator _characterAnimator;

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
        _characterAnimator = Character.Instance.Animator;
    }

    public void GetEnemy(Enemy enemy)
    {
        _enemy = enemy;
    }

    public virtual void Anim_Attack()
    {
        _animator.SetTrigger("Attack1");
    }
    
    public virtual void Anim_DamagePoint() // �������� �ִ� ����
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

    public virtual void Anim_Damaged()
    {
        _animator.SetTrigger("Damaged");
    }
    public virtual void Anim_Jump()
    {
        _animator.SetTrigger("Jump");
    }
    public virtual void Anim_Death()
    {
        _animator.SetTrigger("Death");
    }

    // AI�� ���� ������ ���� ��ȯ
    public virtual void End_Anim() 
    {
        DebugManager.ins.Log("�ش� �ִϸ��̼� ����", DebugManager.TextColor.Yellow);
    }
}
