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
        Normal, // ���� ���� �ϳ�
        Boss, // ���� ���� ������(�⺻, �������� ���)
    }
    public int _hp;
    public int _damage;
}
