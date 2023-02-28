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
    private CharacterController controller; // 캐릭터 움직임 관련 컴포넌트

    private CharacterAnimator animator;

    float _speedSmoothVelocity = 1f;
    float _speedSmoothTime = 1f;
    float _currentSpeed;
    float _velocityY;

    float gravity = 25.0f;
    float moveSpeed = 5.0f; // 5
    float rotateSpeed = 3.0f;

    // Movement
    [SerializeField] private GameObject _rotateActor; // 회전체
    [SerializeField] private GameObject _iroha; // 이로하 모델링 및 애니메이터 포함 객체

    [SerializeField] private Transform _targetPoint;
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

    private void CursurSetting() // 임시니까 그냥 Movement에 박음
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, 0);
    }

    private bool _moveStart; // 걷는 애니메이션 시작함
    private void GetInput()
    {
        if (animator.IsDoing)
            return;
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //if (direction.magnitude >= 0.1f) 
        if (direction.normalized != Vector3.zero)
        {
            // +(조건문 추가) 만약 공격이나 애니메이션 동작중이 아니라면
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

                // 키를 누른 방향으로 _iroha의 회전 변경되도록 해야함 이로하만 y 회전 +90
                // x,z 는 0으로 변환시켜야함
                _iroha.transform.localRotation = Quaternion.Slerp(_iroha.transform.localRotation,
                     target * horizontal, 0.3f);
            }



            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);

            // 움직일때는 TargetPoint 의 x 회전값은 무시하고 y 회전값만 적용되도록 해야함.
            // 왜 why x의 회전값이 바뀌면 캐릭터 이동이 하늘로 뜸
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
        else // 입력이 없는 상태(정규화 벡터로 받아와서 클릭 끊기면 바로 적용되게)
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
        Vector2 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // 십자좌표계
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
        _targetPoint.transform.DOLookAt(_pinEnemy.PinTargetPoint.position, 0.15f, AxisConstraint.None, null).SetEase(Ease.Linear);
        _rotateActor.transform.localRotation = Quaternion.Euler(0, _targetPoint.transform.localEulerAngles.y, 0);
        _saveQuaternion = _targetPoint.localRotation;
        // MouseRotator에서 저장된 Quaternion 값이 Pin 취소되었을 때 돌아가면서 문제가 생김.
        //saveValue 
    }
}
