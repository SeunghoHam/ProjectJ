using Assets.Scripts.Manager;
using UnityEngine;

public class PinTargetRange : RangeSystem
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            BattleManager.AddPinEnemy(other.GetComponent<Enemy>());
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            BattleManager.RemovePinEnemy(other.GetComponent<Enemy>());
        }
    }
}
