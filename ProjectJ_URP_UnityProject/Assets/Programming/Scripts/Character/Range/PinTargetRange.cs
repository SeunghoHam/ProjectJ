using Assets.Scripts.Manager;
using UnityEngine;

public class PinTargetRange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            BattleManager.AddPinEnemy(other.GetComponent<Enemy>());
        }
        //Character.AddEnemy(other.GetComponent<Enemy>());

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            BattleManager.RemovePinEnemy(other.GetComponent<Enemy>());
        }
        //Character.RemoveEnemy(other.GetComponent<Enemy>());
    }
}
