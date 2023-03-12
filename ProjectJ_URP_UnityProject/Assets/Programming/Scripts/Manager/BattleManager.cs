using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class BattleManager : UnitySingleton<BattleManager>
    {
        // 리스트도 여기서 관리해야할까?
        // ex) 캐릭터 공격가능 및 고정 가능 리스트
        private static List<Enemy> _enemyList_pin = new List<Enemy>();
        private static List<Enemy> _enemyList_attack = new List<Enemy>();

        private static List<Enemy> _activeEnemyList = new List<Enemy>();

        private static GameObject _player; // 캐릭터가 범위내에 있다면 공격함

        // 회피같은 시스템을 위해서 캐릭터랑 전투중인 적들 가져오기
        private EnemyMovement enemyMovemnet;

        #region Attack
        public static List<Enemy> GetEnemy()
        {
            return _enemyList_attack;
        }
        public static Enemy AddEnemy(Enemy enemy) // 공격가능한 범위에 적이 들어왔으면 할당
        {
            _enemyList_attack.Add(enemy);
            return enemy;
        }
        public static Enemy RemoveEnemy(Enemy enemy) // 적이 나갓졍
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
        /// 플레이어의 공격이 모션에서 공격에 해당하는 부분에서 재생
        /// </summary>
        public static void Attack() 
        {
            for (int i = 0; i < GetEnemy().Count; i++)
            {
                if (GetEnemy()[i].CanAvoid) // 회피 가능 상태
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
        /// 플레이어 피격
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