using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController controller; // ĳ���� ������ ���� ������Ʈ
    private CharacterAnimator animator;

    float _speedSmoothVelocity = 0.5f; // default 1
    float _speedSmoothTime = 1f;
    float _currentSpeed = 2.5f;
    float _velocityY;

    //float gravity = 25.0f;
    float moveSpeed = 5.0f; // 5
    float rotateSpeed = 10.0f;

    // Movement
    // [SerializeField] private GameObject _rotateActor; // ȸ��ü
    [SerializeField] private GameObject _iroha; // �̷��� �𵨸� �� �ִϸ����� ���� ��ü

    [SerializeField] private Transform _targetPoint;

    private Vector3 direction; // Ű���� �Է�
    private Vector2 mouseAxis;
    private float _rotX;
    private float _rotY;
    private float _sensitivity = 5f;
    private Quaternion _xTargetRotation;
    private Quaternion _yTargetRotation;
    private Quaternion _saveQuaternion;


    // Attack
    private bool _isAttacking; // �������� ���¿����� ������ ����

    // Jump
    //private bool _isJumping; // �������̸� �̵��ӵ� ���ҽ��Ѿ���
    // PinTarget
    private bool _isPin;
    private Enemy _pinEnemy;
    // �� �Ŀ� �ٽ� ���� ���ư��� �� �� �����صα��

    // Roll
    private Vector3 _rollTargetPos;
    private Vector3 _rollMovePos;

    public bool Attacking
    {
        get { return _isAttacking; }
        set
        {
            if (_isAttacking != value)
                _isAttacking = value;
        }
    }
    public bool IsPin
    {
        get { return _isPin; }
        set
        {
            if (_isPin != value)
            {
                if (value)
                {

                }
                else
                {
                    // ������ ���� �� ��
                    //DebugManager.ins.Log("Pin ���� ����", DebugManager.TextColor.Red);
                    //_saveQuaternion = _targetPoint.localRotation;
                }
                _isPin = value; // 
            }
        }
    }
    void Start()
    {
        controller = this.gameObject.GetComponent<CharacterController>();
        animator = Character.Instance.characterAnimator;
        CursurSetting();
        _saveQuaternion = Quaternion.identity;

    }

    private void Update()
    {
        //AboutJump();
        //AboutRoll();
    }
    void FixedUpdate()
    {
        AboutJump();
        AboutRoll();
        GetInput();
        Movement();
        //RollMoving();

        if (!_isPin)
            MouseRotator();
        else
            PinRotator();

        //Move();
        //PlayerMovement();
        //PlayerRotation();
    }

    private void CursurSetting() // �ӽôϱ� �׳� Movement�� ����
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, 0);
    }

    private bool _moveStart; // �ȴ� �ִϸ��̼� ������


    private bool _isMove_Forward;
    private bool _isMove_Back;
    private bool _isMove_Right;
    private bool _isMove_Left;

    private void Move()
    {
        if (_isMove_Forward)
        {

        }
        else if (_isMove_Back)
        {

        }
        else return;

        if (_isMove_Right)
        {

        }
        else if (_isMove_Left)
        {

        }
        else return;
    }

    private void GetInput()
    {
        // +(���ǹ� �߰�) ���� �����̳� �ִϸ��̼� �������� �ƴ϶��
        //if (animator.IsRolling)
        //if(animator.AnimState != CharacterAnimator.ChaAnimState.Idle)
        if (animator.IsRolling || animator.IsAttacking) // �̵� �Ұ� ���¿���
        {
            _currentSpeed = 0f;
            return;
        }
        _currentSpeed = 2.5f;


        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // y ���� ���� ��ġ �Ҵ��غ���
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        //if (direction.magnitude >= 0.1f)
        // if (direction != Vector3.zero)
        if (input != Vector2.zero)
        {
            // ������ �ִ� ��쿡 Aniamtor�� WalkValue�� 0 �ʰ��� ��������
            animator.Anim_GetDirection(input.x, input.y);
            animator.Anim_WalkValue(1);
            if (_isPin) // Ÿ�� ���� Ȱ��ȭ
            {
                if (animator.IsJumping)
                    _currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);
                else
                    _currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed * 0.5f, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);

                controller.Move(_targetPoint.localRotation * direction * _currentSpeed * Time.deltaTime);
                Quaternion target = Quaternion.Euler(0, _targetPoint.transform.localEulerAngles.y, 0);
                _iroha.transform.localRotation = target;

            }
            else // Ÿ�� ���� ��Ȱ��ȭ
            {

                // Ű�� ���� �������� _iroha�� ȸ�� ����ǵ��� �ؾ��� �̷��ϸ� y ȸ�� +90
                Quaternion horizontal = Quaternion.Euler(0, direction.normalized.x * 90f, 0); // �� �� ȸ��
                //Quaternion vertical = Quaternion.Euler(0, direction.normalized.z * -180f, 0); // �� �� ȸ��
                Quaternion vertical = Quaternion.identity;
                
                // �̰�
                
                Quaternion target = Quaternion.Euler(0, _targetPoint.transform.localEulerAngles.y, 0); // ���� �ٶ󺸰��ִ� ������ ��������
                _iroha.transform.localRotation = Quaternion.Slerp(_iroha.transform.localRotation,
                       target * horizontal * vertical, rotateSpeed * Time.deltaTime);


                // �����϶��� TargetPoint �� x ȸ������ �����ϰ� y ȸ������ ����ǵ��� �ؾ���.
                // �� why x�� ȸ������ �ٲ�� ĳ���� �̵��� �ϴ÷� ��
                //_currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);

                controller.Move(_yTargetRotation * direction * _currentSpeed * Time.deltaTime);

                /*
                if (!_moveStart)
                {
                    _moveStart = true;
                    animator.Anim_Move();
                }*/

                //DebugManager.ins.Log("curSpeed : " + _currentSpeed, DebugManager.TextColor.White);
            }
        }
        else // �Է��� ���� ����(����ȭ ���ͷ� �޾ƿͼ� Ŭ�� ����� �ٷ� ����ǰ�)
        {
            animator.Anim_GetDirection(0,0);
            animator.Anim_WalkValue(0);
            //Debug.Log("��ֶ����� false");
            /*
            if (_moveStart)
            {
                _moveStart = false;
                animator.Anim_Idle();
            }*/

            //if (_currentSpeed <= 0)
            //return;
            //_currentSpeed = Mathf.SmoothDamp(_currentSpeed, 0, ref _decSmoothVelocitt, _speedSmoothTime * Time.deltaTime); // �ӵ� 0���� ����� ���ؼ�
            //_currentSpeed = 0f;

        }

    }
    private void Movement()
    {
        controller.Move(_yTargetRotation // ���콺 ȸ��(targetPoint)
            * direction // �ٶ󺸴� ����
            * _currentSpeed // ���� �ӵ�
            * Time.deltaTime);

        //if(animator.IsJumping)
            controller.Move(_jumpDir * Time.deltaTime);

        //if(animator.IsRolling)
     
        // �ִϸ��̼� ����
        
    }
    private void MouseRotator()
    {
        mouseAxis.x = Input.GetAxis("Mouse X");
        mouseAxis.y = Input.GetAxis("Mouse Y");
        //= new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // ������ǥ��
        _rotX += (mouseAxis.x * _sensitivity);
        _rotY += (mouseAxis.y * _sensitivity);

        _rotY = Mathf.Clamp(_rotY, -80f, 80f); // ����ȸ�� �ִ밪 ����
        _yTargetRotation = Quaternion.Euler(0, _rotX, 0); // �¿�ȸ��
        _xTargetRotation = Quaternion.Euler(_rotY * 0.8f, 0, 0); // ����ȸ�� : ���� ������ �ʿ��غ���

        _targetPoint.transform.localRotation = (_yTargetRotation * _xTargetRotation) * _saveQuaternion;
        // TargetPoint �� EulerAngles  ���� = ( _rotY,  rotX, 0 );
    }
    public void SetPinEnemy(Enemy enemy)
    {
        if (enemy == null)
        {
            _pinEnemy.Targeting(false);
            _pinEnemy = null;
        }
        else
        {
            _pinEnemy = enemy;
            enemy.Targeting(true);
        }
    }
    private void PinRotator()
    {
        // Debug.Log("PinRotator");
        _targetPoint.transform.DOLookAt(_pinEnemy.PinTargetPoint.position, 0.1f, AxisConstraint.Y
            , null).SetEase(Ease.Linear);

        Quaternion yValue = Quaternion.Euler(0, _targetPoint.localEulerAngles.y, 0);
        //_rotateActor.transform.localRotation =
        _iroha.transform.localRotation = Quaternion.Slerp(_iroha.transform.localRotation, yValue, 1f);

        // MouseRotator���� ����� Quaternion ���� Pin ��ҵǾ��� �� ���ư��鼭 ������ ����.
        //saveValue 
    }

    public void RollMove()
    {
        _rollDir = new Vector3(_iroha.transform.forward.x,
            _iroha.transform.position.y,
            _iroha.transform.forward.z);

        //_rollTargetPos = _iroha.transform.forward * 2.5f + this.transform.position; // �̷��� ���� ������ �������� �������� �ؾ���

        //_rollMovePos = new Vector3(_rollTargetPos.x, this.transform.position.y , _rollTargetPos.z);
        //_rollDir = controller.transform.TransformDirection(_rollMovePos); // ���ú��͸� ���庤�ͷ� ��ȯ��Ŵ

        /*
        vCurPos = this.transform.position;
        Vector3 vDist = vTargetPos - vCurPos;
        Vector3 vDir = vDist.normalized;
        float fDist = vDist.magnitude; // ���͸� ����ȭ �Ͽ� �Ÿ��� ����
        */
        //this.transform.DOMove(_rollMovePos, 0.8f, false).SetEase(Ease.InQuad); // snapping true = �����̵�
    }
    private Vector3 _rollDir; // ������ ����
    private float _rollSpeed = 30f;
    private void AboutRoll()
    {
        if(animator.IsRolling) // ��������
        {
            //controller.Move(_rollDir * Time.deltaTime);
            controller.Move(_rollDir * Time.deltaTime);
        }
        else
        {

        }
    }
    private float _jumpHeight = 1.5f;
    private float _jumpLinearValue = 30f;

    private Vector3 _jumpDir; // �߷� ����
    private float _gravityScale = 15f; // �߷�ũ��

    public void Jump()
    {
        DebugManager.ins.Log("����", DebugManager.TextColor.Blue);
        _jumpDir = controller.transform.TransformDirection(_jumpDir); // ���ú��͸� ���庤�ͷ� ��ȯ��Ŵ
    }
    private void AboutJump()
    {
        if (controller.isGrounded)
        {
            //_moveDir.y = _jumpHeight;
        }
        else
        {
            _jumpDir.y -= _gravityScale * Time.deltaTime;
        }
        if(animator.IsJumping) // �������� �� ����ϵ���
        {
            //_jumpDir.y = Mathf.Lerp(_jumpDir.y, _jumpHeight, _jumpLinearValue * Time.deltaTime);
            _jumpDir.y = _jumpHeight;
        }
    }
}