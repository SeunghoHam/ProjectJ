using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinTargetRange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
            return;
        Character.AddEnemy(other.GetComponent<Enemy>());
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Enemy"))
            return;
        Character.RemoveEnemy(other.GetComponent<Enemy>());
    }
}
