using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : UnitBase
{
    // ĳ���� �� ������ ���� �޾ƿ���
    public EnemyData _data; // ScriptAble�����ͷ� ����� ���� ��������
    public EnemyAnimator animator;
    public EnemyMovement movement;

    public EnemyBTBase enemyBT;

    /*
    public enum EnemyType
    {
        Normal, // ���� ���� 1��(�⺻)
        Boss, 
    }
    public EnemyType enemyType;*/

    [SerializeField] private List<EnemyAttackRange> enemyAttackRange = new List<EnemyAttackRange>();

    [Space(20)]
    [Header("�÷��̾� ���� ����")]
    [SerializeField] private Transform _pinTarget; // ������ų ������Ʈ(�ٶ󺸴°�)
    [SerializeField] private MeshRenderer _pinObject; // �����Ǿ��ִٸ� Ȱ��ȭ ��ų ������Ʈ (�׽�Ʈ����)


    [Space(10)]
    [Header("���� ��Ȳ")]
    [SerializeField] private int _hp;


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

    /// <summary> EnemyData ���� ������ ������ �Ҵ��ϱ� </summary>
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
    #region Battle
    public override void Attack()
    {
        
        if( _data._type == EnemyData.EnemyType.Boss)
        {
            // � �������� ���������
            int attack = 0;
            switch (attack)
            {
                case 0:
                    // �⺻ ��Ÿ
                    RangeType(EnemyAttackRange.RangeType.Melee);
                    break;
                case 1:
                    // ��������
                    RangeType(EnemyAttackRange.RangeType.Jump);
                    break;
                default:
                    RangeType(EnemyAttackRange.RangeType.Melee);
                    break;
            }
        }
        animator.Anim_Attack1();
        base.Attack();
    }
    private void RangeType(EnemyAttackRange.RangeType rangeType)
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
