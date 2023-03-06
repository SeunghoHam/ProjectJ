using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // PinTarget
    public PinTargetRange pinTargetRange;
    public CharacterMovement characterMovement;
    public CharacterAnimator characterAnimator;
    public static Character Instance;
    public static List<Enemy> _enemyList = new List<Enemy>();

    // CharacterState : Normal / Sword
    public enum WeaponState
    { 
        Normal,
        Sword,
    }
    public WeaponState weaponState = WeaponState.Normal;

    [Space(10)]
    public Transform TargetPoint;

    private void Awake()
    {
        Instance = this;
    }
    public static Enemy GetEnemy()
    {
        return _enemyList[0];
    }
    public static Enemy AddEnemy(Enemy enemy)
    {
        _enemyList.Add(enemy);
        //DebugManager.ins.Log("리스트에 추가. 현재 리스트 개수 : " + _enemyList.Count, DebugManager.TextColor.Blue);
        return enemy;
    }

    public static Enemy RemoveEnemy(Enemy enemy)
    {
        _enemyList.Remove(enemy);
        //DebugManager.ins.Log("리스트에서 삭제. 현재 리스트 개수 : " + _enemyList.Count, DebugManager.TextColor.Red);
        return enemy;
    }
}
