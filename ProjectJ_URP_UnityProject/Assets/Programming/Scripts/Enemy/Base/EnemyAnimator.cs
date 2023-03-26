using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Enemy _enemy;
    [SerializeField] protected EnemyMovement _movement; // 특정 애니메이션 동작에서 이동시키기 위해서 movement 할당

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


    // 캐릭터의 회피 여부 반환을 위해 할당
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
    
    public virtual void Anim_DamagePoint() // 데미지를 넣는 순간
    {
        if(_enemy.CanHit)
        {
            if (_characterAnimator.IsRolling)
            {
                DebugManager.ins.Log("회피 성공", DebugManager.TextColor.Blue);
            }
            else 
            {
                DebugManager.ins.Log("회피 실패", DebugManager.TextColor.Red);
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

    // AI의 다음 동작을 위해 반환
    public virtual void End_Anim() 
    {
        DebugManager.ins.Log("해당 애니메이션 종료", DebugManager.TextColor.Yellow);
    }
}
