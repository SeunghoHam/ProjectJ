using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // PinTarget
    public PinTargetRange pinTargetRange;
    public static Character Instance;
    public static List<Enemy> _enemyList = new List<Enemy>();

    private void Awake()
    {
        Instance = this;
    }
    public static Enemy AddEnemy(Enemy enemy)
    {
        _enemyList.Add(enemy);
        DebugManager.ins.Log("����Ʈ�� �߰�. ���� ����Ʈ ���� : " + _enemyList.Count, DebugManager.TextColor.Blue);
        return enemy;
    }

    public static Enemy RemoveEnemy(Enemy enemy)
    {
        _enemyList.Remove(enemy);
        DebugManager.ins.Log("����Ʈ���� ����. ���� ����Ʈ ���� : " + _enemyList.Count, DebugManager.TextColor.Red);
        return enemy;
    }
}
