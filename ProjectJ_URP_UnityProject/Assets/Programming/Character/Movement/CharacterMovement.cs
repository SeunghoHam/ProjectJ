using AmplifyShaderEditor;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController controller; // 캐릭터 움직임 관련 컴포넌트

    private CharacterAnimator animator;

    float _speedSmoothVelocity = 1f;
    float _speedSmoothTime = 1f;
    float _currentSpeed;
    float _velocityY;

    //float gravity = 25.0f;
    float moveSpeed = 5.0f; // 5
    //float rotateSpeed = 3.0f;

    // Movement
    [SerializeField] private GameObject _rotateActor; // 회전체
    [SerializeField] private GameObject _iroha; // 이로하 모델링 및 애니메이터 포함 객체

    [SerializeField] private Transform _targetPoint;

    private Vector3 direction; // 키보드 입력
    private Vector2 mouseAxis;
    private float _rotX;
    private float _rotY;
    private float _sensitivity = 5f;
    Quaternion _xTargetRotation;
    Quaternion _yTargetRotation;


    // Attack
    private bool _isAttacking; // 공격중인 상태에서는 움직임 막기

    // PinTarget
    private bool _isPin;
    private Enemy _pinEnemy;
    // 핀 후에 다시 상태 돌아갔을 때 값 저장해두기용
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
                    // 고정이 해제 될 때
                    DebugManager.ins.Log("Pin 상태 해제", DebugManager.TextColor.Red);

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
        //Move();
        //PlayerMovement();
        //PlayerRotation();
    }

    private void CursurSetting() // 임시니까 그냥 Movement에 박음
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, 0);
    }

    private bool _moveStart; // 걷는 애니메이션 시작함


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
        // +(조건문 추가) 만약 공격이나 애니메이션 동작중이 아니라면
        if (animator.IsDoing)
            return;

        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (direction.magnitude >= 0.1f)
        //if (direction != Vector3.zero)
        {
            if (_isPin) // 타겟 고정 활성화
            {
                _currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);
                controller.Move(_targetPoint.localRotation * direction * _currentSpeed * Time.deltaTime);


                if(direction.normalized.x == 1) // →
                {

                }
                else if(direction.normalized.x == -1) // ←
                {

                }
            }
            else // 타겟 고정 비활성화
            {
                if (direction.normalized.z != 0)
                {
                    /*
                    if (direction.normalized.z == 1) // 앞
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
                       target * vertical, 2f * Time.deltaTime);


                }

                if (direction.normalized.x != 0)
                {
                    Quaternion horizontal = Quaternion.Euler(0, direction.normalized.x * 90f, 0);
                    Quaternion target = Quaternion.Euler(0, _targetPoint.transform.localEulerAngles.y, 0);

                    // 키를 누른 방향으로 _iroha의 회전 변경되도록 해야함 이로하만 y 회전 +90
                    // x,z 는 0으로 변환시켜야함
                    _iroha.transform.localRotation = Quaternion.Slerp(_iroha.transform.localRotation,
                         target * horizontal, 0.1f);
                }


                // 움직일때는 TargetPoint 의 x 회전값은 무시하고 y 회전값만 적용되도록 해야함.
                // 왜 why x의 회전값이 바뀌면 캐릭터 이동이 하늘로 뜸
                _currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);
                controller.Move(_yTargetRotation * direction * _currentSpeed * Time.deltaTime);

                //_iroha.transform.localRotation = Quaternion.Lerp(_iroha.transform.localRotation, _yTargetRotation, 0.1f); // 임시 주석
                _rotateActor.transform.localRotation = Quaternion.Lerp(_rotateActor.transform.localRotation, _yTargetRotation, 0.2f);
                //controller.Move(_targetPoint.localRotation * direction * _currentSpeed * Time.deltaTime);
                //controller.Move(_rotateActor.transform.localRotation * direction * _currentSpeed * Time.deltaTime);
                if (!_moveStart)
                {
                    _moveStart = true;
                    animator.Anim_Move();
                }
     
            }
            //if (_velocityY > -10) _velocityY -= Time.deltaTime * gravity;
        }
        else // 입력이 없는 상태(정규화 벡터로 받아와서 클릭 끊기면 바로 적용되게)
        {
            Debug.Log("노멀라이즈 false");
            if (_moveStart)
            {
                _moveStart = false;
                animator.Anim_Idle();
            }
        }

    }
    private void MouseRotator()
    {
        mouseAxis.x = Input.GetAxis("Mouse X");
        mouseAxis.y = Input.GetAxis("Mouse Y");
        //= new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // 십자좌표계
        _rotX += (mouseAxis.x * _sensitivity);
        _rotY += (mouseAxis.y * _sensitivity);

        _rotY = Mathf.Clamp(_rotY, -40f, 40f); // 상하회전 최대값 제한
        _yTargetRotation = Quaternion.Euler(0, _rotX, 0); // 좌우회전
        _xTargetRotation = Quaternion.Euler(_rotY, 0, 0); // 상하회전

        _targetPoint.transform.localRotation = _yTargetRotation * _xTargetRotation; //* _saveQuaternion;
        // TargetPoint 의 EulerAngles  상태 = ( _rotY,  rotX, 0 );
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
        _rotateActor.transform.localRotation =  Quaternion.Slerp(_rotateActor.transform.localRotation, yValue, 1f);

        //_saveQuaternion = _targetPoint.localRotation;
        // MouseRotator에서 저장된 Quaternion 값이 Pin 취소되었을 때 돌아가면서 문제가 생김.
        //saveValue 
    }
}
