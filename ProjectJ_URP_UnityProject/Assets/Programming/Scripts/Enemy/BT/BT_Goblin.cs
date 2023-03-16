using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Goblin : EnemyBTBase
{
    private void Awake()
    {
        Debug.Log("고블린 AI 활성화");
        movement = this.GetComponent<EnemyMovement>();
    }

    protected override IEnumerator AIRoutineBody() 
    {
        movement.AI_Doing_Jump();
        Debug.Log("AiRoutine");
        yield return new WaitForSeconds(1f);
        Debug.Log("1초 기다렸다가 반환");
        // Base실행 전에  스킬 실행해야함
        yield return base.AIRoutineBody();
    }

    private IEnumerator Doing_JumpAttack()
    {
        yield return null;  
    }
}
