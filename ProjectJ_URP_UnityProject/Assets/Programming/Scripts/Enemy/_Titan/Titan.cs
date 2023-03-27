using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Titan : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Attack()
    {
        if (_data._type == EnemyData.EnemyType.Boss)
        {
            // 어떤 공격인지 정해줘야함
            switch (_attackNumber)
            {
                case 0:
                    // 기본 평타
                    RangeType(EnemyAttackRange.RangeType.Melee);
                    animator.Anim_Attack();
                    break;
                case 1:
                    // 점프공격
                    RangeType(EnemyAttackRange.RangeType.Jump);
                    //movement.AI_Doing_Jump();
                    animator.Anim_Jump(); // 애니메이터에서 점프 시작할 시점 할당함
                    break;
                case 2:
                    // 이동공격
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
