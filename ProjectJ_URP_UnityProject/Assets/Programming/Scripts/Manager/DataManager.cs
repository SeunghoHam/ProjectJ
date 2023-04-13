using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Application = UnityEngine.Device.Application;

namespace Assets.Scripts.Manager
{
    public class DataManager : UnitySingleton<DataManager>
    {
        // 저장 경로 반환
        //private string savePath = Application.persistentDataPath;
        
        public void DataSave()
        {
            
            PlayerPrefs.SetString("userName", Character.Instance.userName);
            PlayerPrefs.SetInt("level", 1);
            PlayerPrefs.SetFloat("exp", Character.Instance.exp);
        }
        
        public void DataLoad()
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