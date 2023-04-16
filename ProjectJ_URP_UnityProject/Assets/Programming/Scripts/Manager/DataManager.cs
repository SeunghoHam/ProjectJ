using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;
using System.IO;

namespace Assets.Scripts.Manager
{
    public class DataManager : UnitySingleton<DataManager>
    {
        // PlayerPrefs저장 경로 반환
        //private string savePath = Application.persistentDataPath;
        

        private string filePath;
        private PlayerData _playerData;

        private void Awake()
        {
            filePath = Path.Combine(Application.dataPath, "Resources/PlayerData/", "PlayerData.json");
            JsonLoad();
            //JsonSave(true);
        }

        #region ::: Runes :::

        public void GetRunes(int runesAmount)
        {
            //PlayerData playerData = new PlayerData();
            // 증가는 그냥 시키고?
            // 저장을 한번에 받는게 좋을듯
            _playerData.playerRunes += runesAmount;
        }
        
        #endregion
        public void JsonLoad()
        {
            //PlayerData playerData = new PlayerData();
            if (!File.Exists(filePath)) // json 파일이 비어있다면 처음부터 생성함
            {
                Debug.Log("PlayerData X");
                JsonSave(false);
            }
            else // 비어있지 않은 경우
            {
                //JsonSave(true);
                Debug.Log("PlayerData O");
                string loadJson = File.ReadAllText(filePath);
                _playerData = JsonUtility.FromJson<PlayerData>(loadJson);
                // 값 할당시키기
                if (_playerData != null)
                {
                    Debug.Log("캐릭터 인스턴스에 값 전달");
                    // 이 부분에서 전달해야함
                    
                    //playerData.inventoryItems = new List<string>() { "item3", "item4" };
                }
            }
        }
        public void JsonSave(bool isOn)
        {
            DebugManager.ins.Log("playerData [SAVE]",DebugManager.TextColor.Yellow);
            //_playerData //;= new PlayerData();
            if (!isOn)
            {
                Debug.Log("새로운 값으로 저장");
                NewCharacterCreate();
            }
            
            string json = JsonUtility.ToJson(_playerData, true);
            File.WriteAllText(filePath, json);
            DataToCharacterInstance();
            Debug.Log("플레이어 데이터 : " +  _playerData.playerName);
            //Debug.Log("저장 완료" + json.ToString());
        }

        // 처음 게임을 키게 되면 유저가 하게 해야하는데 일단 바로 적용되도록 함
        private void NewCharacterCreate()
        {
            _playerData = new PlayerData();
            _playerData.playerName = "Jungi";
            _playerData.playerExp = 1;
            _playerData.playerRunes = 1000;
            Debug.Log("플레이어 데이터 : " +  _playerData);
        }

        private void DataToCharacterInstance()
        {
            Character.Instance.plyaerName = _playerData.playerName;
            Character.Instance.playerExp = _playerData.playerExp;
            Character.Instance.playerRunes = _playerData.playerRunes;
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