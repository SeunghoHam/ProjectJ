using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : UnitBase
{
    // ĳ���� �� ������ ���� �޾ƿ���
    public EnemyData _data; // ScriptAble�����ͷ� ����� ���� ��������
    public EnemyAnimator enemyAnimator;
    public EnemyMovement enemyMovement;


    [Header("�÷��̾� ���� ����")]
    [SerializeField] private Transform _pinTarget;
    [SerializeField] private Image _pinImage;


    [Space(10)]
    [Header("���� ��Ȳ")]
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
            Debug.Log("���� �ְŽ�");
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
    public void Targeting(bool value) // Ÿ�ټ�����
    {
        _pinImage.gameObject.SetActive(value);
    }
    public Transform PinTargetPoint // ĳ�����ʿ��� Ÿ�ٰ����� ���
    {
        get { return _pinTarget; }
    }

    #endregion
}
