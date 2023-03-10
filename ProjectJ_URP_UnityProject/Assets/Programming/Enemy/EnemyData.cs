using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    public string _name;
    public Rare _rare;
    public enum Rare
    { 
        Normal,
        Rare,
        Boss,
    }

    public int _hp;
    public int _damage;
}
