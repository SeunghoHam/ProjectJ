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
    }

    private void EnemyInitilaize()
    {
        enemyMovement.Initialize(enemyAnimator);
        _pinImage.gameObject.SetActive(false);
        _hp = _data._hp;

    }
    private void Update()
    {
        if (_pinImage.gameObject.activeSelf)
            _pinImage.transform.LookAt(Camera.main.transform);
    }

    #region Battle
    public override void Attack()
    {
        base.Attack();
    }
    public override void Damaged(int damage)
    {
        base.Damaged(damage);
        _hp -= damage;

        if (_hp <= 0)
        {
            Debug.Log("적이 주거써");
            enemyAnimator.Anim_Death();
        }
        //else return;
    }
    public override void Avoid()
    {
        base.Avoid();
        enemyMovement.Avoid();
    }
    #endregion


    #region TargetPin
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
