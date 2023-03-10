using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class BattleManager : UnitySingleton<BattleManager>
    {
        // ����Ʈ�� ���⼭ �����ؾ��ұ�?
        // ex) ĳ���� ���ݰ��� �� ���� ���� ����Ʈ
        private static List<Enemy> _enemyList_pin = new List<Enemy>();
        private static List<Enemy> _enemyList_attack = new List<Enemy>();


        #region Attack
        public static List<Enemy> GetEnemy()
        {
            return _enemyList_attack;
        }
        public static Enemy AddEnemy(Enemy enemy) // ���ݰ����� ������ ���� �������� �Ҵ�
        {
            _enemyList_attack.Add(enemy);
            return enemy;
        }
        public static Enemy RemoveEnemy(Enemy enemy) // ���� ������
        {
            _enemyList_attack.Remove(enemy);
            return enemy;
        }
        


        #endregion

        private void Awake()
        {

        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void UnInitialize()
        {
            base.UnInitialize();
        }
    }
}