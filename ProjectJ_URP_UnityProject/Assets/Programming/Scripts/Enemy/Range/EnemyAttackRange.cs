using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    private Enemy _enemy;
    private BoxCollider _boxCollider;
    public enum RangeType
    {
        Melee, // 일반 공격
        Jump, // 점프 후 착지공격
    }
    public RangeType rangeType;

    public void GetEnemy(Enemy enemy)
    {
        _enemy = enemy;
        _boxCollider = this.GetComponent<BoxCollider>();
    }
    public void SetColliderEnable(bool ison)
    {
        DebugManager.ins.Log(this.name + "active : " + ison,DebugManager.TextColor.White);
        _boxCollider.enabled = ison;
    }
    private void OnTriggerEnter(Collider other)
    {
        _enemy.CanHit = true;
    }
    private void OnTriggerExit(Collider other)
    {
        _enemy.CanHit = false;
    }
}
