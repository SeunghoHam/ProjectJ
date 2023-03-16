using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EnemyBTBase : MonoBehaviour
{
    protected EnemyMovement movement;
    public void BT_Setting()
    {
        if (AIRoutine != null)
        {
            StopCoroutine(AIRoutine);
            AIRoutine = null;
        }

        AIRoutine = AIRoutineBody();
        StartCoroutine(AIRoutine);
    }

    private void Awake()
    {
        
    }
    protected IEnumerator AIRoutine; // �̷��� �޾ƾ� StopCoroutine �� ����
    protected virtual IEnumerator AIRoutineBody()
    { 
        // ���� ���� ������ �ؾ��ϴ°� ( Ÿ������ �̹� �Ǿ��ִ»���)
        yield return null; // Base������ �ؾ���
    }

    protected void Doing_Idle() // ���� ���Ǳ�
    {

    }
}
