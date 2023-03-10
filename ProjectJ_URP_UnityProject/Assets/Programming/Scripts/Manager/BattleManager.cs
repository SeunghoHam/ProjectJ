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