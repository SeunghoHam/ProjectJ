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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            DebugManager.ins.Log("AI동작 : 점프", DebugManager.TextColor.Yellow);
            StartCoroutine(movement.AI_Doing_Jump());
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            DebugManager.ins.Log("AI동작 : 롤링", DebugManager.TextColor.Yellow);
            StartCoroutine(movement.AI_Doing_Rolling());
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            DebugManager.ins.Log("AI동작 : 점프공격", DebugManager.TextColor.Yellow);
            StartCoroutine(movement.AI_Doing_JumpAttack());
        }
    }
    protected override IEnumerator AIRoutineBody() 
    {
        
        int ran = UnityEngine.Random.Range(0, 2);
        switch (ran)
        {
            case 0:
                movement.AI_Doing_Jump();
                break;
            case 1:
                break;
            default:
                break;
        }
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
