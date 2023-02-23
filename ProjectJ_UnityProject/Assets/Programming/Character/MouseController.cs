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
  
    [SerializeField] private Transform _target; //  ī�޶� �ٶ󺸴� ���

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
        // ���콺 ���οø� x + / ���콺 �Ʒ��� ���� x -
        // ���콺 ���������� y + / ���콺 ��������  y -

        // ���콺�� ���οø��� y�� ����
        // ���콺�� ���������� x�� ����
        Vector2 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // ������ǥ��
        _rotX += (mouseAxis.x * _sensitivity);
        _rotY += (mouseAxis.y * _sensitivity);

        _rotY = Mathf.Clamp(_rotY, -30f, 30f);
        Quaternion localRotation = Quaternion.Euler(_rotY, _rotX, 0);
        // ���� ������ ���߿� �ٽ� ������
        //_target.transform.localRotation = Quaternion.Slerp(_target.transform.localRotation, localRotation, Time.deltaTime * _rotateSmooth);
        _target.transform.localRotation = localRotation;
    }
}
