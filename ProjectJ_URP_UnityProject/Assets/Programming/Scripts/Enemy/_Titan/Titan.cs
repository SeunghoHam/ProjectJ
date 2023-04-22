using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Titan : Enemy
{
    public override void Attack()
    {
        if (_data._type == EnemyData.EnemyType.Boss)
        {
            // � �������� ���������
            switch (_attackNumber)
            {
                case 0:
                    // �⺻ ��Ÿ
                    RangeType(EnemyAttackRange.RangeType.Melee);
                    animator.Anim_Attack();
                    break;
                case 1:
                    // ��������
                    RangeType(EnemyAttackRange.RangeType.Jump);
                    //movement.AI_Doing_Jump();
                    animator.Anim_Jump(); // �ִϸ����Ϳ��� ���� ������ ���� �Ҵ���
                    break;
                case 2:
                    // �̵�����
                    RangeType(EnemyAttackRange.RangeType.Melee);
                    
                    break;
                default:
                    RangeType(EnemyAttackRange.RangeType.Melee);
                    break;
            }
        }
        base.Attack();
    }
}
