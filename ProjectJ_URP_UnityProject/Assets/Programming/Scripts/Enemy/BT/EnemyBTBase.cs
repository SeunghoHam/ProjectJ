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
    protected IEnumerator AIRoutine; // 이렇게 받아야 StopCoroutine 이 먹음
    protected virtual IEnumerator AIRoutineBody()
    { 
        // 적은 무슨 동작을 해야하는가 ( 타겟팅은 이미 되어있는상태)
        yield return null; // Base이전에 해야함
    }

    protected void Doing_Idle() // 상태 살피기
    {

    }
}
