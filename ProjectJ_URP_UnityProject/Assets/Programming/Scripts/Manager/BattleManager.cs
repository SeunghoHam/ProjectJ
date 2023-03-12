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

        private static List<Enemy> _activeEnemyList = new List<Enemy>();

        private static GameObject _player; // ĳ���Ͱ� �������� �ִٸ� ������

        // ȸ�ǰ��� �ý����� ���ؼ� ĳ���Ͷ� �������� ���� ��������
        private EnemyMovement enemyMovemnet;

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

        public static GameObject GetPlayer()
        {
            return _player;
        }
        public static GameObject AddPlayer(GameObject player)
        {
            _player = player;
            return player;
        }
        public static void RemovePlayer()
        {
            _player = null;
        }

        #endregion

        /// <summary>
        /// �÷��̾��� ������ ��ǿ��� ���ݿ� �ش��ϴ� �κп��� ���
        /// </summary>
        public static void Attack() 
        {
            for (int i = 0; i < GetEnemy().Count; i++)
            {
                if (GetEnemy()[i].CanAvoid) // ȸ�� ���� ����
                {
                    GetEnemy()[i].Avoid();
                }
                else
                {
                    GetEnemy()[i].Damaged(Character.Instance.AttackDamage);
                }
            }
        }

        /// <summary>
        /// �÷��̾� �ǰ�
        /// </summary>
        public static void Damaged()
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