using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{ 
    private CharacterController controller; // ĳ���� ������ ���� ������Ʈ

    private CharacterAnimator animator;
    //Animator anim;
    
    float _speedSmoothVelocity = 1f;
    float _speedSmoothTime = 1f;
    float _currentSpeed;
    float _velocityY;

    float gravity = 25.0f;
    float moveSpeed = 5.0f;
    float rotateSpeed = 3.0f;

    // Movement
    [SerializeField] private GameObject _rotateActor;
    [SerializeField] private Transform _targetPoint;
    private float _rotX;
    private float _rotY;
    private float _sensitivity = 5f;
    Quaternion _xTargetRotation;
    Quaternion _yTargetRotation;

    private bool _isAttacking; // �������� ���¿����� ������ ����

    public bool Attacking
    {
        get { return _isAttacking; }
        set
        {
            if (_isAttacking != value)
                _isAttacking = value;
        }
    }
    void Start()
    {
        controller = this.gameObject.GetComponent<CharacterController>();
        animator = Character.Instance.characterAnimator;
        CursurSetting();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        //PlayerMovement();
        //PlayerRotation();
        MouseRotator();
    }

    private void CursurSetting() // �ӽôϱ� �׳� Movement�� ����
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, 0);
    }
    private bool _moveStart; // �ȴ� �ִϸ��̼� ������
    private void GetInput()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // �̵� ���� ��������.


        //if (direction.magnitude >= 0.1f) 
        if (direction.normalized != Vector3.zero)
        {
            // +(���ǹ� �߰�) ���� �����̳� �ִϸ��̼� �������� �ƴ϶��
            if (direction.normalized.z == 1) // ����ȭ�� ������(������ ������)
            {
                _rotateActor.transform.localRotation = Quaternion.Lerp(_rotateActor.transform.localRotation, _yTargetRotation, 0.3f);
            }
            else if (direction.normalized.z == -1)
            {
                // ī�޶� �ٶ󺸰� �ڷ� ����
            }

            if (direction.normalized.x == 1) // ��
            {
            }
            else if (direction.normalized.x == -1) // ��
            {
            }
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);
            controller.Move(_rotateActor.transform.localRotation * direction * _currentSpeed * Time.deltaTime);

            if(!_moveStart)
            {
                _moveStart = true;
                animator.Anim_Move();
            }
        }
        else // �Է��� ���� ����(����ȭ ���ͷ� �޾ƿͼ� Ŭ�� ����� �ٷ� ����ǰ�)
        {
            if(_moveStart)
            {
                _moveStart = false;
                animator.Anim_Idle();
            }
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

        _targetPoint.transform.localRotation = _yTargetRotation * _xTargetRotation; // ��Ŀ����̱⿡ �������� ���̵ȴ�.
    }

}
