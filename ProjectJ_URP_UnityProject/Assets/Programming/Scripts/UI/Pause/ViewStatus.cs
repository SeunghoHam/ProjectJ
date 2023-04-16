using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Manager;
using UnityEngine;
using UnityEngine.UI;

public class ViewStatus : MonoBehaviour
{
    private DataManager _dataManager;
    
    [SerializeField] private Text _playerName; 
    [SerializeField] private Text _playerLevel;
    [SerializeField] private Text _playerRunes;

    //private PlayerData _playerdata;

    public void GetDataManager(DataManager dataManager)
    {
        
        _dataManager = dataManager;
    }
    private void OnEnable()
    {
        //_dataManager.JsonSave(true);
        //DataManager.Instance.JsonSave(true);
        //DataManager.Instance.JsonLoad();
        Debug.Log(("Status Enable"));
        _playerName.text = Character.Instance.plyaerName;
        _playerLevel.text = Character.Instance.playerExp.ToString();
        _playerRunes.text = Character.Instance.playerRunes.ToString();
        
        ExpToLevel();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("룬 획득");
            _dataManager.GetRunes(1);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            _dataManager.JsonSave(true);
        }
    }

    private void ExpToLevel()
    {
        //float exp = Character.Instance.exp;
        //_playerLevel.text = Character.Instance.exp.ToString();
    }
}
