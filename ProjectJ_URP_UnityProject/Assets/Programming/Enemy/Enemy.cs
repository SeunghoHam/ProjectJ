using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemyInfo _data;
    [SerializeField] private Transform _pinTarget;
    [SerializeField] private Image _pinImage;
    
    private void Awake()
    {
        _pinImage.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (_pinImage.gameObject.activeSelf)
            _pinImage.transform.LookAt(Camera.main.transform);
    }
    public void Targeting(bool value) // Å¸°Ù¼³Á¤µÊ
    {
        _pinImage.gameObject.SetActive(value);
    }

    public Transform PinTargetPoint
    {
        get
        {
            return _pinTarget;
        }
    }
   
}
