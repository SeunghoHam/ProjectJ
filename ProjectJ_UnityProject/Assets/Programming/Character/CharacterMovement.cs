using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    MouseController _mouseController;
    CharacterController controller;
    //Animator anim;
    Transform cam;
    float _speedSmoothVelocity = 1f;
    float _speedSmoothTime = 1f;
    float _currentSpeed;
    float _velocityY;
    Vector3 _moveInput;
    Vector3 _dir;

    float gravity = 25.0f;
    float moveSpeed = 3.0f;
    float rotateSpeed = 3.0f;

    [SerializeField] private GameObject _rotateActor;
    [SerializeField] private Transform _target;

    void Start()
    {
        _mouseController = this.GetComponent<MouseController>();
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
        InputAction();

        
    }
    private void LateUpdate()
    {
    }

    private void CursurSetting() // �ӽôϱ� �׳� Movement�� ����
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, 0);
    }
    private void GetInput()
    {

        _moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized; // �̵� ���� ��������
        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;
        forward.y = 0;
        right.y = 0;

        if (_moveInput.x == 0 && _moveInput.y == 0) // �Է��� ���� ����
        {
            _dir = Vector3.zero;
        }
        else // _dir ����
        {
            _dir = (forward * _moveInput.y + right * _moveInput.x); // normalized �ϸ� 1 / 0 �� �����ϰ� �Ǽ� �� �� ����
                                                                    // ���� �����̳� �ִϸ��̼� �������� �ƴ϶�� : ���ǹ� �߰������

            //Quaternion newRot = Quaternion.Euler(0, _target.transform.localRotation.y, 0); // target���� y���� �������� ���ؼ�
            _rotateActor.transform.localRotation = Quaternion.Lerp(_rotateActor.transform.localRotation, _target.transform.localRotation, 1f);
            // �̵��� target�� �ٶ󺸴� �������� ����ǵ���
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);
            //Vector3 velocity = (_dir * _currentSpeed) + Vector3.up * _velocityY; // ���� �̵����

            //controller.Move(_rotateActor.transform.localRotation* direction * Time.deltaTime);
        }
        if (_velocityY > -10) _velocityY -= Time.deltaTime * gravity;
        //Vector3 forward = cam.forward;
        //Vector3 right = cam.right;

        //forward.Normalize();
        //right.Normalize();

        // �̵� ������ target�� RotValue�� �������� ��
    }
    private void PlayerMovement()
    {
    


        // �ִϸ��̼� ���� 
        //anim.SetFloat("Movement", _dir.magnitude, 0.1f, Time.deltaTime);
    }
    private void PlayerRotation()
    {
        if (_dir.magnitude == 0) return;
        Vector3 rotDir = new Vector3(_dir.x, _dir.y, _dir.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotDir), Time.deltaTime);
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
