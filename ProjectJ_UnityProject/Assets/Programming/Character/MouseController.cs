using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UniRx.Triggers;
using UnityEditor.Timeline;
using System.Runtime.InteropServices.WindowsRuntime;

public class MouseController : MonoBehaviour
{

    private float _rotX;
    private float _rotY;
    private float _sensitivity = 5f;

    float _rotateSmooth = 1f; 
  
    [SerializeField] private Transform _target; //  카메라가 바라보는 대상

    public Vector3 TargetValue()
    {
        return _target.transform.localEulerAngles;
    }
    private void Update()
    {
        MouseRotator();
    }
    private void MouseRotator()
    {
        // 마우스 위로올림 x + / 마우스 아래로 내림 x -
        // 마우스 오른쪽으로 y + / 마우스 왼쪽으로  y -

        // 마우스를 위로올리면 y가 증가
        // 마우스를 오른쪽으로 x가 증가
        Vector2 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // 십자좌표계
        _rotX += (mouseAxis.x * _sensitivity);
        _rotY += (mouseAxis.y * _sensitivity);

        _rotY = Mathf.Clamp(_rotY, -30f, 30f);
        Quaternion localRotation = Quaternion.Euler(_rotY, _rotX, 0);
        // 감도 설정은 나중에 다시 들어가야함
        //_target.transform.localRotation = Quaternion.Slerp(_target.transform.localRotation, localRotation, Time.deltaTime * _rotateSmooth);
        _target.transform.localRotation = localRotation;
    }
}
