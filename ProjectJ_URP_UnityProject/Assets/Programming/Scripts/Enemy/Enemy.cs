using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : UnitBase
{
    // ĳ���� �� ������ ���� �޾ƿ���
    public EnemyData _data; // ScriptAble�����ͷ� ����� ���� ��������
    public EnemyAnimator animator;
    public EnemyMovement movement;

    [HideInInspector] 
    public EnemyBTBase enemyBT;


    [Space(20)]
    [Header("�÷��̾� ���� ����")]
    [SerializeField] private Transform _pinTarget; // ������ų ������Ʈ(�ٶ󺸴°�)
    [SerializeField] private MeshRenderer _pinObject; // �����Ǿ��ִٸ� Ȱ��ȭ ��ų ������Ʈ


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
        movement.Initialize(animator);
        _pinObject.enabled = false;
        _hp = _data._hp;
        enemyBT = this.GetComponent<EnemyBTBase>();
    }

    public void BattleStart()
    {
        Debug.Log("���� ����");

        // AI Ȱ��ȭ
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
            Debug.Log("���� �ְŽ�");
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


    #region ::: TargetPin - ĳ���Ϳ��� ��� :::
    public void Targeting(bool value) // Ÿ�ټ�����
    {
        _pinObject.enabled = value;
    }
    public Transform PinTargetPoint // ĳ�����ʿ��� Ÿ�ٰ����� ���
    {
        get { return _pinTarget; }
    }

    #endregion
}
