using System.Collections.Generic;
using UnityEngine;
public class Enemy : UnitBase
{
    // 캐릭터 및 데이터 정보 받아오기
    public EnemyData _data; // ScriptAble데이터로 저장된 정보 가져오기
    public EnemyAnimator animator;
    public EnemyMovement movement;

    public EnemyBTBase enemyBT;

    /// Attack함수에 모든 종류의 공격을 할당하기 위해서 공격을 따로 받음
    protected int _attackNumber;

    // 공격 범위 할당하기
    // ㄴ보스같이 범위가 많다면 많이 할당되고, 잡몹이라면 한개만 들어감 
    [SerializeField] protected List<EnemyAttackRange> enemyAttackRange = new List<EnemyAttackRange>();

    [Space(10)]
    [Header("보스 상황")]
    [SerializeField] private int _hp;

    [Space(10)]
    [Header("플레이어 시점 고정")]
    [SerializeField] private Transform _pinTarget; // 고정시킬 오브젝트(바라보는거)
    [SerializeField] private MeshRenderer _pinObject; // 고정되어있다면 활성화 시킬 오브젝트 (테스트용임)




    private bool _canHit = false;
    public bool CanHit
    {
        get { return _canHit; }
        set 
        {
            if (_canHit != value)
                _canHit = value;
        }
    }

    private bool _canAvoid = false;
    public bool CanAvoid
    {
        get { return _canAvoid; }
        set
        {
            if (_canAvoid != value)
                _canAvoid = value;
        }
    }

    private void Awake()
    {
        EnemyInitilaize();
    }

    /// <summary> EnemyData 에서 가져온 정보로 할당하기 </summary>
    private void EnemyInitilaize()
    {
        movement.Initialize(animator);
        animator.GetEnemy(this);
        _pinObject.enabled = false;
        _hp = _data._hp;
        enemyBT = this.GetComponent<EnemyBTBase>();

        // RangeSetting
        for (int i = 0; i < enemyAttackRange.Count; i++)
        {
            enemyAttackRange[i].GetEnemy(this);
        }
    }
    public void ChangeAttackNumber(int number)
    {
        _attackNumber = number;
    }

    #region Battle
    protected void RangeType(EnemyAttackRange.RangeType rangeType)
    {
        for (int i = 0; i < enemyAttackRange.Count; i++)
        {
            if (enemyAttackRange[i].rangeType == rangeType)
            {
                enemyAttackRange[i].SetColliderEnable(true);
            }
            else
                enemyAttackRange[i].SetColliderEnable(false);
        }
    }
    public override void Attack()
    {
        base.Attack();
    }
    public override void Damaged(int damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            Debug.Log("적이 주거써");
            animator.Anim_Death();
            this.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            animator.Anim_Damaged();
        }

        base.Damaged(damage);
    }

    public override void Avoid()
    {
        base.Avoid();
        movement.AI_Doing_Avoid();
    }

    #endregion


    #region ::: TargetPin - 캐릭터에서 사용 :::
    public void Targeting(bool value) // 타겟설정됨
    {
        _pinObject.enabled = value;
    }
    public Transform PinTargetPoint // 캐릭터쪽에서 타겟고정에 사용
    {
        get { return _pinTarget; }
    }
    #endregion
}
