using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Goblin : EnemyBTBase
{
    private void Awake()
    {
        Debug.Log("��� AI Ȱ��ȭ");
        movement = this.GetComponent<EnemyMovement>();
    }

    protected override IEnumerator AIRoutineBody() 
    {
        movement.AI_Doing_Jump();
        Debug.Log("AiRoutine");
        yield return new WaitForSeconds(1f);
        Debug.Log("1�� ��ٷȴٰ� ��ȯ");
        // Base���� ����  ��ų �����ؾ���
        yield return base.AIRoutineBody();
    }

    private IEnumerator Doing_JumpAttack()
    {
        yield return null;  
    }
}
