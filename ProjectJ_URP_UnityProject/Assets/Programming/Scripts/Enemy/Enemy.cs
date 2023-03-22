using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : UnitBase
{
    // 캐릭터 및 데이터 정보 받아오기
    public EnemyData _data; // ScriptAble데이터로 저장된 정보 가져오기
    public EnemyAnimator animator;
    public EnemyMovement movement;

    [HideInInspector] 
    public EnemyBTBase enemyBT;


    [Space(20)]
    [Header("플레이어 시점 고정")]
    [SerializeField] private Transform _pinTarget; // 고정시킬 오브젝트(바라보는거)
    [SerializeField] private MeshRenderer _pinObject; // 고정되어있다면 활성화 시킬 오브젝트


    [Space(10)]
    [Header("보스 상황")]
    [SerializeField] private int _hp;

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
        BattleStart();
    }
    private void EnemyInitilaize()
    {
        movement.Initialize(animator);
        _pinObject.enabled = false;
        _hp = _data._hp;
        enemyBT = this.GetComponent<EnemyBTBase>();
    }

    public void BattleStart()
    {
        Debug.Log("전투 시작");

        // AI 활성화
        //enemyBT.BT_Setting();
    }
    #region Battle
    public override void Attack()
    {
        animator.Anim_Attack1();
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
