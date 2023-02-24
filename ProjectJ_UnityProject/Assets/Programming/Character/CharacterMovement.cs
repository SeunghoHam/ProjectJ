using AmplifyShaderEditor;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{ 
    CharacterController controller;
    //Animator anim;
    Transform cam;
    float _speedSmoothVelocity = 1f;
    float _speedSmoothTime = 1f;
    float _currentSpeed;
    float _velocityY;

    float gravity = 25.0f;
    float moveSpeed = 5.0f;
    float rotateSpeed = 3.0f;

    [SerializeField] private GameObject _rotateActor;
    [SerializeField] private Transform _target;

    private float _rotX;
    private float _rotY;
    private float _sensitivity = 5f;

    Quaternion _xTargetRotation;
    Quaternion _yTargetRotation;
    void Start()
    {
        controller = this.gameObject.GetComponent<CharacterController>();
        cam = Camera.main.transform;
        CursurSetting();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        //PlayerMovement();
        //PlayerRotation();
        MouseRotator();
        InputAction();

        
    }

    private void CursurSetting() // �ӽôϱ� �׳� Movement�� ����
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, 0);
    }
    private void GetInput()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // �̵� ���� ��������. normalize �����ϱ�
        /*
        if (direction.x == 0 && direction.z == 0) // �Է��� ���� ����
        {
            direction = Vector3.zero;
        }
        else // ���� ����*/
        {
            // +(���ǹ� �߰�) ���� �����̳� �ִϸ��̼� �������� �ƴ϶��
            if (direction.normalized.z == 1) // ����ȭ�� ������(������ ������)
            {
                DebugManager.ins.Log("��", DebugManager.TextColor.Yellow);
                _rotateActor.transform.localRotation = Quaternion.Lerp(_rotateActor.transform.localRotation, _yTargetRotation, 0.3f);
            }
            else if (direction.normalized.z == -1)
            {
                // ī�޶� �ٶ󺸰� �ڷ� ����
                DebugManager.ins.Log("��", DebugManager.TextColor.Yellow);
            }

            if (direction.normalized.x == 1)
            {
                DebugManager.ins.Log("��", DebugManager.TextColor.Blue);
            }
            else if (direction.normalized.x == -1)
            {
                DebugManager.ins.Log("��", DebugManager.TextColor.Blue);
                
            }

            // _dir = (forward * _moveInput.y + right * _moveInput.x); // ����ȭ(normalize) �ϸ� 1 / 0 �� �����ϰ� �Ǽ� �� �� ����
            // Quaternion�� �����ؾ� ȸ���� ����ϰ� ��(EulerAngles�Ұ�)
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);
            controller.Move(_rotateActor.transform.localRotation * direction * _currentSpeed * Time.deltaTime);
        }
        if (_velocityY > -10) _velocityY -= Time.deltaTime * gravity;
    }

    private void MouseRotator()
    {
        Vector2 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // ������ǥ��
        _rotX += (mouseAxis.x * _sensitivity);
        _rotY += (mouseAxis.y * _sensitivity);

        _rotY = Mathf.Clamp(_rotY, -30f, 30f);
        _yTargetRotation = Quaternion.Euler(0, _rotX, 0); // �¿�ȸ��
        _xTargetRotation = Quaternion.Euler(_rotY, 0, 0); // ����ȸ��

        _target.transform.localRotation = _yTargetRotation * _xTargetRotation; // ��Ŀ����̱⿡ �������� ���̵ȴ�.
    }
    private void InputAction()
    {
        // ��ó�� ���� ���ٸ� Ÿ������ ���� �ʴ´�.
        if (Input.GetKeyDown(KeyCode.Q))
        {
             DebugManager.ins.Log("Ÿ���� ����", DebugManager.TextColor.Blue);
        }


    }
}
