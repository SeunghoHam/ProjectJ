using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewStatus : MonoBehaviour
{
    [SerializeField] private Text _playerName; 
    [SerializeField] private Text _playerLevel; 
    private void OnEnable()
    {
        _playerName.text = Character.Instance.userName;
        ExpToLevel();
    }

    private void ExpToLevel()
    {
        //float exp = Character.Instance.exp;
        _playerLevel.text = Character.Instance.exp.ToString();
    }
}
