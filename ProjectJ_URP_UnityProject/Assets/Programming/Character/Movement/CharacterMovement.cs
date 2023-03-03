using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController controller; // ĳ���� ������ ���� ������Ʈ

    private CharacterAnimator animator;

    float _speedSmoothVelocity = 1f;
    float _speedSmoothTime = 1f;
    float _currentSpeed;
    float _velocityY;

    //float gravity = 25.0f;
    float moveSpeed = 5.0f; // 5
    float rotateSpeed = 10.0f;

    // Movement
    [SerializeField] private GameObject _rotateActor; // ȸ��ü
    [SerializeField] private GameObject _iroha; // �̷��� �𵨸� �� �ִϸ����� ���� ��ü

    [SerializeField] private Transform _targetPoint;

    private Vector3 direction; // Ű���� �Է�
    private Vector2 mouseAxis;
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
                    // ������ ���� �� ��
                    DebugManager.ins.Log("Pin ���� ����", DebugManager.TextColor.Red);

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


    void FixedUpdate()
    {
        GetInput();
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
        if (animator.IsDoing)
            return;

        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (direction.magnitude >= 0.1f)
        //if (direction != Vector3.zero)
        {
            if (_isPin) // Ÿ�� ���� Ȱ��ȭ
            {
                _currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);
                controller.Move(_targetPoint.localRotation * direction * _currentSpeed * Time.deltaTime);


                if (direction.normalized.x == 1) // ��
                {

                }
                else if (direction.normalized.x == -1) // ��
                {

                }
            }
            else // Ÿ�� ���� ��Ȱ��ȭ
            {
                // ĳ���� ȸ�� �� �� ������ ����ǰ� �ؾ���
                if (direction.normalized.z != 0) 
                {
                    /*
                    if (direction.normalized.z == 1) // ��
                    {
                         vertical = Quaternion.identity;
                          target = Quaternion.Euler(0, _targetPoint.transform.localEulerAngles.y, 0);
                    }
                    else
                    {
                        vertical = Quaternion.Euler(0, direction.normalized.z +  180f, 0);
                        target = Quaternion.Euler(0, _targetPoint.transform.localEulerAngles.y, 0);
                    }*/
                    Quaternion vertical = Quaternion.identity;
                    Quaternion target = Quaternion.Euler(0, _targetPoint.transform.localEulerAngles.y, 0);
                    _iroha.transform.localRotation = Quaternion.Slerp(_iroha.transform.localRotation,
                       target * vertical, rotateSpeed * Time.deltaTime);


                }

                if (direction.normalized.x != 0)
                {
                    Quaternion horizontal = Quaternion.Euler(0, direction.normalized.x * 90f, 0);
                    Quaternion target = Quaternion.Euler(0, _targetPoint.transform.localEulerAngles.y, 0);

                    // Ű�� ���� �������� _iroha�� ȸ�� ����ǵ��� �ؾ��� �̷��ϸ� y ȸ�� +90
                    // x,z �� 0���� ��ȯ���Ѿ���
                    _iroha.transform.localRotation = Quaternion.Slerp(_iroha.transform.localRotation,
                         target * horizontal, rotateSpeed * Time.deltaTime);
                }


                // �����϶��� TargetPoint �� x ȸ������ �����ϰ� y ȸ������ ����ǵ��� �ؾ���.
                // �� why x�� ȸ������ �ٲ�� ĳ���� �̵��� �ϴ÷� ��
                _currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);
                controller.Move(_yTargetRotation * direction * _currentSpeed * Time.deltaTime);

                //_iroha.transform.localRotation = Quaternion.Lerp(_iroha.transform.localRotation, _yTargetRotation, 0.1f); // �ӽ� �ּ�
                _rotateActor.transform.localRotation = Quaternion.Slerp(_rotateActor.transform.localRotation, _yTargetRotation,1f);
                //controller.Move(_targetPoint.localRotation * direction * _currentSpeed * Time.deltaTime);
                //controller.Move(_rotateActor.transform.localRotation * direction * _currentSpeed * Time.deltaTime);
                /*
                if (!_moveStart)
                {
                    _moveStart = true;
                    animator.Anim_Move();
                }*/

            }
            //if (_velocityY > -10) _velocityY -= Time.deltaTime * gravity;
        }
        else // �Է��� ���� ����(����ȭ ���ͷ� �޾ƿͼ� Ŭ�� ����� �ٷ� ����ǰ�)
        {
            //Debug.Log("��ֶ����� false");
            /*
            if (_moveStart)
            {
                _moveStart = false;
                animator.Anim_Idle();
            }*/
        }

    }
    private void MouseRotator()
    {
        mouseAxis.x = Input.GetAxis("Mouse X");
        mouseAxis.y = Input.GetAxis("Mouse Y");
        //= new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // ������ǥ��
        _rotX += (mouseAxis.x * _sensitivity);
        _rotY += (mouseAxis.y * _sensitivity);

        _rotY = Mathf.Clamp(_rotY, -40f, 40f); // ����ȸ�� �ִ밪 ����
        _yTargetRotation = Quaternion.Euler(0, _rotX, 0); // �¿�ȸ��
        _xTargetRotation = Quaternion.Euler(_rotY * 0.8f, 0, 0); // ����ȸ�� : ���� ������ �ʿ��غ���

        _targetPoint.transform.localRotation = _yTargetRotation * _xTargetRotation;
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
        Debug.Log("PinRotator");
        _targetPoint.transform.DOLookAt(_pinEnemy.PinTargetPoint.position, 0.1f, AxisConstraint.Y
            , null).SetEase(Ease.Linear);

        Quaternion yValue = Quaternion.Euler(0, _targetPoint.localEulerAngles.y, 0);
        //_rotateActor.transform.localRotation =
        _rotateActor.transform.localRotation = Quaternion.Slerp(_rotateActor.transform.localRotation, yValue, 1f);

        //_saveQuaternion = _targetPoint.localRotation;
        // MouseRotator���� ����� Quaternion ���� Pin ��ҵǾ��� �� ���ư��鼭 ������ ����.
        //saveValue 
    }

    Vector3 vTargetPos;
    Vector3 vCurPos;
    Vector3 vDist;
    Vector3 vDir;
    float fDist;
    float _rollSpeed = 2f;
    public void RollMove()
    {
        Vector3 vTargetPos =  _iroha.transform.forward * 2.5f + this.transform.position; // �̵���ų ����
        Vector3 vMovePos = new Vector3(vTargetPos.x, 0, vTargetPos.z);
        /*
        vCurPos = this.transform.position;
        Vector3 vDist = vTargetPos - vCurPos;
        Vector3 vDir = vDist.normalized;
        float fDist = vDist.magnitude; // ���͸� ����ȭ �Ͽ� �Ÿ��� ����
        */
        this.transform.DOMove(vTargetPos, 0.8f, false).SetEase(Ease.InQuad); // snapping true = �����̵�
        
    }
}
