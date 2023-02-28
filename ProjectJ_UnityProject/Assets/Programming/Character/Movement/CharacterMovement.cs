using AmplifyShaderEditor;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using Unity.VisualScripting;
//using System.Numerics;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController controller; // ĳ���� ������ ���� ������Ʈ

    private CharacterAnimator animator;

    float _speedSmoothVelocity = 1f;
    float _speedSmoothTime = 1f;
    float _currentSpeed;
    float _velocityY;

    float gravity = 25.0f;
    float moveSpeed = 5.0f; // 5
    float rotateSpeed = 3.0f;

    // Movement
    [SerializeField] private GameObject _rotateActor; // ȸ��ü
    [SerializeField] private GameObject _iroha; // �̷��� �𵨸� �� �ִϸ����� ���� ��ü

    [SerializeField] private Transform _targetPoint;
    private float _rotX;
    private float _rotY;
    private float _sensitivity = 5f;
    Quaternion _xTargetRotation;
    Quaternion _yTargetRotation;


    // Attack
    private bool _isAttacking; // �������� ���¿����� ������ ����

    // PinTarget
    private bool _isPin;
    private Enemy _pinEnemy;
    // �� �Ŀ� �ٽ� ���� ���ư��� �� �� �����صα��
    private Quaternion _saveQuaternion = Quaternion.identity;
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

                }
                _isPin = value;
            }
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
        if (!_isPin)
            MouseRotator();
        else
            PinRotator();
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
    private void GetInput()
    {
        if (animator.IsDoing)
            return;
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //if (direction.magnitude >= 0.1f) 
        if (direction.normalized != Vector3.zero)
        {
            // +(���ǹ� �߰�) ���� �����̳� �ִϸ��̼� �������� �ƴ϶��
            if (direction.normalized.z != 0 )
            {
                Quaternion vertical = Quaternion.Euler(0, direction.normalized.y * 90f, 0);
                Quaternion target = Quaternion.Euler(0, _targetPoint.transform.localEulerAngles.y, 0);

                _iroha.transform.localRotation = Quaternion.Slerp(_iroha.transform.localRotation,
                   target * vertical, 0.3f);
            }

            if(direction.normalized.x !=0 )
            {
                Quaternion horizontal = Quaternion.Euler(0, direction.normalized.x * 90f, 0);
                Quaternion target = Quaternion.Euler(0, _targetPoint.transform.localEulerAngles.y, 0);

                // Ű�� ���� �������� _iroha�� ȸ�� ����ǵ��� �ؾ��� �̷��ϸ� y ȸ�� +90
                // x,z �� 0���� ��ȯ���Ѿ���
                _iroha.transform.localRotation = Quaternion.Slerp(_iroha.transform.localRotation,
                     target * horizontal, 0.3f);
            }



            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);

            // �����϶��� TargetPoint �� x ȸ������ �����ϰ� y ȸ������ ����ǵ��� �ؾ���.
            // �� why x�� ȸ������ �ٲ�� ĳ���� �̵��� �ϴ÷� ��
            controller.Move(_yTargetRotation * direction * _currentSpeed * Time.deltaTime);
            //_iroha.transform.localRotation = Quaternion.Lerp(_iroha.transform.localRotation, _yTargetRotation, 0.1f); // �ӽ� �ּ�
            _rotateActor.transform.localRotation = Quaternion.Lerp(_rotateActor.transform.localRotation, _yTargetRotation, 0.2f);
            //controller.Move(_targetPoint.localRotation * direction * _currentSpeed * Time.deltaTime);
            //controller.Move(_rotateActor.transform.localRotation * direction * _currentSpeed * Time.deltaTime);


            if (!_moveStart)
            {
                _moveStart = true;
                animator.Anim_Move();
            }
        }
        else // �Է��� ���� ����(����ȭ ���ͷ� �޾ƿͼ� Ŭ�� ����� �ٷ� ����ǰ�)
        {
            if (_moveStart)
            {
                _moveStart = false;
                animator.Anim_Idle();
            }
        }
        //if (_velocityY > -10) _velocityY -= Time.deltaTime * gravity;
    }
    private void MouseRotator()
    {
        Vector2 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // ������ǥ��
        _rotX += (mouseAxis.x * _sensitivity);
        _rotY += (mouseAxis.y * _sensitivity);

        _rotY = Mathf.Clamp(_rotY, -40f, 40f); // ����ȸ�� �ִ밪 ����
        _yTargetRotation = Quaternion.Euler(0, _rotX, 0); // �¿�ȸ��
        _xTargetRotation = Quaternion.Euler(_rotY, 0, 0); // ����ȸ��

        _targetPoint.transform.localRotation = _yTargetRotation * _xTargetRotation; //* _saveQuaternion;
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
        _targetPoint.transform.DOLookAt(_pinEnemy.PinTargetPoint.position, 0.15f, AxisConstraint.None, null).SetEase(Ease.Linear);
        _rotateActor.transform.localRotation = Quaternion.Euler(0, _targetPoint.transform.localEulerAngles.y, 0);
        _saveQuaternion = _targetPoint.localRotation;
        // MouseRotator���� ����� Quaternion ���� Pin ��ҵǾ��� �� ���ư��鼭 ������ ����.
        //saveValue 
    }
}
