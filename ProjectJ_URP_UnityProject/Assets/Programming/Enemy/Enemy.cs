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

    [HideInInspector] 
    public EnemyBTBase enemyBT;


    [Space(20)]
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
        //DebugManager.ins.Log("���� ����", DebugManager.TextColor.Yellow);
        Debug.Log("���� ����");
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
            Debug.Log("���� �ְŽ�");
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


    #region ::: TargetPin - ĳ���Ϳ��� ��� :::
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
