using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    public string _name;
    public EnemyType _type;
    public enum EnemyType
    { 
        Normal, // 공격 범위 하나
        Boss, // 공격 범위 여러개(기본, 점프공격 등등)
    }
    public int _hp;
    public int _damage;
}
