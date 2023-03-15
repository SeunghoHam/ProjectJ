using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController controller; // ĳ���� ������ ���� ������Ʈ
    private CharacterAnimator animator;
    private CameraSystem cameraSystem;

    float _speedSmoothVelocity = 0.5f; // default 1
    float _speedSmoothTime = 1f;
    float _currentSpeed = 2.5f;
    float _velocityY;

    float moveSpeed = 5.0f; // 5
    float rotateSpeed = 10.0f;

    // Movement
    [SerializeField] private GameObject _iroha; // �̷��� �𵨸� �� �ִϸ����� ���� ��ü

    [SerializeField] private Transform _targetPoint;

    private float _walkValue; // �ȴ� ��.
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
        cameraSystem = Character.Instance.cameraSystem;
        CursurSetting();
        _saveQuaternion = Quaternion.identity;
    }

    void FixedUpdate()
    {
        AboutJump();
        AboutRoll();
        GetInput();
        Movement();

        if (!_isPin)
            MouseRotator();
        else
            PinRotator();

        cameraSystem._targetPoint_xValue = _targetPoint.localRotation.x;
        //cameraSystem._targetPoint_xValue = _xTargetRotation.x;
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
        if (animator.AnimState == CharacterAnimator.ChaAnimState.Roll ||
            animator.AnimState == CharacterAnimator.ChaAnimState.Attack ||
            animator.AnimState == CharacterAnimator.ChaAnimState.SeriesAttackReady
            ) // �̵� �Ұ� ���¿���
        {
            _currentSpeed = 0f;
            return;
        }
        _currentSpeed = 2.5f;

        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // y ���� ���� ��ġ �Ҵ��غ���
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        if (input != Vector2.zero)
        {
            // ������ �ִ� ��쿡 Aniamtor�� WalkValue�� 0 �ʰ��� ��������
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

                Quaternion target = Quaternion.Euler(0, _targetPoint.transform.localEulerAngles.y, 0); // ���� �ٶ󺸰��ִ� ������ ��������
                _iroha.transform.localRotation = Quaternion.Slerp(_iroha.transform.localRotation,
                       target * horizontal * vertical, rotateSpeed * Time.deltaTime);

                // �����϶��� TargetPoint �� x ȸ������ �����ϰ� y ȸ������ ����ǵ��� �ؾ���.
                // �� why x�� ȸ������ �ٲ�� ĳ���� �̵��� �ϴ÷� ��
                //_currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);
            }
        }
        else // �Է��� ���� ����(����ȭ ���ͷ� �޾ƿͼ� Ŭ�� ����� �ٷ� ����ǰ�)
        {

        }
    }

    private void Movement()
    {
        if (animator.ReturnCanMove())
        {
            controller.Move(_yTargetRotation // ���콺 ȸ��(targetPoint)
            * direction // �ٶ󺸴� ����
            * _currentSpeed // ���� �ӵ�
            * Time.deltaTime);
        }

        // ���� ����
        controller.Move(_jumpDir * Time.deltaTime);

        // �ִϸ��̼� ����
        _walkValue = Mathf.Abs(direction.x + direction.z); // � �����̵� �Է��� �ִٸ� 0 �ʰ��� ����
        animator.Anim_GetDirection(direction.x, direction.z);
        animator.Anim_WalkValue(_walkValue);
    }
    private void MouseRotator()
    {
        mouseAxis.x = Input.GetAxis("Mouse X");
        mouseAxis.y = Input.GetAxis("Mouse Y");
        //= new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // ������ǥ��

        _rotX += (mouseAxis.x * _sensitivity);
        _rotY += (mouseAxis.y * _sensitivity);

        _rotY = Mathf.Clamp(_rotY, -50f, 50f); // ����ȸ�� �ִ밪 ����
        _yTargetRotation = Quaternion.Euler(0, _rotX, 0); // �¿�ȸ��
        _xTargetRotation = Quaternion.Euler(_rotY * 0.8f, 0, 0); // ����ȸ�� : ���� ������ �ʿ��غ���

        _targetPoint.transform.localRotation = (_yTargetRotation * _xTargetRotation) * _saveQuaternion;
        //DebugManager.ins.Log("TargetPoint.y : " + _targetPoint.transform.localEulerAngles.x);
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
        _targetPoint.transform.DOLookAt(_pinEnemy.PinTargetPoint.position, 0.1f, AxisConstraint.Y
            , null).SetEase(Ease.Linear);

        Quaternion yValue = Quaternion.Euler(0, _targetPoint.localEulerAngles.y, 0);
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
    private float _rollSpeed = 2f;
    private void AboutRoll()
    {
        if (animator.IsRolling) // ��������
        {
            //controller.Move(_rollDir * Time.deltaTime);
            controller.Move(_rollDir * _rollSpeed * Time.deltaTime);
        }
        else
        {

        }
    }
    private float _jumpHeight = 1.5f;
    //private float _jumpLinearValue = 30f;

    private Vector3 _jumpDir; // �߷� ����
    private float _gravityScale = 15f; // �߷�ũ��

    public void Jump()
    {
        DebugManager.ins.Log("����", DebugManager.TextColor.Blue);
        _jumpDir = controller.transform.TransformDirection(_jumpDir); // ���ú��͸� ���庤�ͷ� ��ȯ��Ŵ
    }
    private bool JumpProperty
    {
        get { return controller.isGrounded; }
        set
        {
            if (controller.isGrounded != value)
            {
                
            }
        }
    }
    private void AboutJump()
    {
        JumpProperty = controller.isGrounded;

        if (controller.isGrounded)
        {
            //_moveDir.y = _jumpHeight;
            //_jumpDir = Vector3.zero;
        }
        else
        {
            _jumpDir.y -= _gravityScale * Time.deltaTime;
        }
        if (animator.IsJumping) // �������� �� ����ϵ���
        {
            //_jumpDir.y = Mathf.Lerp(_jumpDir.y, _jumpHeight, _jumpLinearValue * Time.deltaTime);
            _jumpDir.y = _jumpHeight;
        }
    }
}