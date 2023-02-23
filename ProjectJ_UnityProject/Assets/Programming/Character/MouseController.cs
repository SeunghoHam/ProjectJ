using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    private float _rotX;
    private float _rotY;
    private float _sensitivity = 10f;

    float _rotateSmooth = 1f; 
    private Vector2 _clampAxis = new Vector2(-40,40);

    [SerializeField] private GameObject _rotateActor;
    void Update()
    {
        MouseRotator();
    }

    private void MouseRotator()
    {
        Vector2 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        _rotX += (mouseAxis.x * _sensitivity) * Time.deltaTime;
        _rotY -= (mouseAxis.y * _sensitivity) * Time.deltaTime;

        _rotY = Mathf.Clamp(_rotY, _clampAxis.x, _clampAxis.y);
        Quaternion localRotation = Quaternion.Euler(_rotY, _rotX, 0);
        _rotateActor.transform.rotation = Quaternion.Slerp(_rotateActor.transform.rotation, localRotation, Time.deltaTime * _rotateSmooth);
    }
}
