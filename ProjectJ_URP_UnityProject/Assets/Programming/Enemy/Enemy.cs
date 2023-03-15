using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : UnitBase
{
    // 캐릭터 및 데이터 정보 받아오기
    public EnemyData _data; // ScriptAble데이터로 저장된 정보 가져오기
    public EnemyAnimator enemyAnimator;
    public EnemyMovement enemyMovement;

    [HideInInspector] 
    public EnemyBTBase enemyBT;


    [Space(20)]
    [Header("플레이어 시점 고정")]
    [SerializeField] private Transform _pinTarget;
    [SerializeField] private Image _pinImage;


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
        enemyMovement.Initialize(enemyAnimator);
        _pinImage.gameObject.SetActive(false);
        _hp = _data._hp;
        enemyBT = this.GetComponent<EnemyBTBase>();
    }

    public void BattleStart()
    {
        //DebugManager.ins.Log("전투 시작", DebugManager.TextColor.Yellow);
        Debug.Log("전투 시작");
        enemyBT.BT_Setting();
    }
    #region Battle
    public override void Attack()
    {
        enemyAnimator.Anim_Slash();
        base.Attack();
    }
    public override void Damaged(int damage)
    {
        _hp -= damage;

        if (_hp <= 0)
        {
            Debug.Log("적이 주거써");
            enemyAnimator.Anim_Death();
        }
        base.Damaged(damage);
        //else return;
    }

    public override void Avoid()
    {
        base.Avoid();
        enemyMovement.AI_Doing_Avoid();
    }

    #endregion


    #region ::: TargetPin - 캐릭터에서 사용 :::
    public void Targeting(bool value) // 타겟설정됨
    {
        _pinImage.gameObject.SetActive(value);
    }
    public Transform PinTargetPoint // 캐릭터쪽에서 타겟고정에 사용
    {
        get { return _pinTarget; }
    }

    #endregion
}
