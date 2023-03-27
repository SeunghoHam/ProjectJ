using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackRange : RangeSystem
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            BattleManager.AddEnemy(other.GetComponent<Enemy>());
        }
        else return;
    }
    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            BattleManager.RemoveEnemy(other.GetComponent<Enemy>());
        }
        else return;
    }
}
