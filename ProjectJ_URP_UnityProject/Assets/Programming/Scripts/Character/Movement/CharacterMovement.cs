using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController controller; // 캐릭터 움직임 관련 컴포넌트
    private CharacterAnimator animator;
    private CameraSystem cameraSystem;

    float _speedSmoothVelocity = 0.5f; // default 1
    float _speedSmoothTime = 1f;
    float _currentSpeed = 2.5f;
    float _velocityY;

    float moveSpeed = 5.0f; // 5
    float rotateSpeed = 10.0f;

    // Movement
    [SerializeField] private GameObject _iroha; // 이로하 모델링 및 애니메이터 포함 객체

    [SerializeField] private Transform _targetPoint;

    private float _walkValue; // 걷는 힘.
    private Vector3 direction; // 키보드 입력
    private Vector2 mouseAxis;
    private float _rotX;
    private float _rotY;
    private float _sensitivity = 5f;
    private Quaternion _xTargetRotation;
    private Quaternion _yTargetRotation;
    private Quaternion _saveQuaternion;


    // Attack
    private bool _isAttacking; // 공격중인 상태에서는 움직임 막기

    // Jump
    //private bool _isJumping; // 점프중이면 이동속도 감소시켜야함
    // PinTarget
    private bool _isPin;
    private Enemy _pinEnemy;
    // 핀 후에 다시 상태 돌아갔을 때 값 저장해두기용

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
                    // 고정이 해제 될 때
                    //DebugManager.ins.Log("Pin 상태 해제", DebugManager.TextColor.Red);
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

    private void CursurSetting() // 임시니까 그냥 Movement에 박음
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
            ) // 이동 불가 상태에서
        {
            _currentSpeed = 0f;
            return;
        }
        _currentSpeed = 2.5f;

        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // y 값에 현재 위치 할당해보기
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        if (input != Vector2.zero)
        {
            // 동작이 있는 경우에 Aniamtor의 WalkValue를 0 초과로 만들어야함
            if (_isPin) // 타겟 고정 활성화
            {
                if (animator.IsJumping)
                    _currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);
                else
                    _currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed * 0.5f, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);

                controller.Move(_targetPoint.localRotation * direction * _currentSpeed * Time.deltaTime);
                Quaternion target = Quaternion.Euler(0, _targetPoint.transform.localEulerAngles.y, 0);
                _iroha.transform.localRotation = target;
            }
            else // 타겟 고정 비활성화
            {
                // 키를 누른 방향으로 _iroha의 회전 변경되도록 해야함 이로하만 y 회전 +90
                Quaternion horizontal = Quaternion.Euler(0, direction.normalized.x * 90f, 0); // 좌 우 회전
                //Quaternion vertical = Quaternion.Euler(0, direction.normalized.z * -180f, 0); // 앞 뒤 회전
                Quaternion vertical = Quaternion.identity;

                Quaternion target = Quaternion.Euler(0, _targetPoint.transform.localEulerAngles.y, 0); // 현재 바라보고있는 시점을 기준으로
                _iroha.transform.localRotation = Quaternion.Slerp(_iroha.transform.localRotation,
                       target * horizontal * vertical, rotateSpeed * Time.deltaTime);

                // 움직일때는 TargetPoint 의 x 회전값은 무시하고 y 회전값만 적용되도록 해야함.
                // 왜 why x의 회전값이 바뀌면 캐릭터 이동이 하늘로 뜸
                //_currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);
            }
        }
        else // 입력이 없는 상태(정규화 벡터로 받아와서 클릭 끊기면 바로 적용되게)
        {

        }
    }

    private void Movement()
    {
        if (animator.ReturnCanMove())
        {
            controller.Move(_yTargetRotation // 마우스 회전(targetPoint)
            * direction // 바라보는 방향
            * _currentSpeed // 현재 속도
            * Time.deltaTime);
        }

        // 점프 정의
        controller.Move(_jumpDir * Time.deltaTime);

        // 애니메이션 관련
        _walkValue = Mathf.Abs(direction.x + direction.z); // 어떤 방향이든 입력이 있다면 0 초과로 나옴
        animator.Anim_GetDirection(direction.x, direction.z);
        animator.Anim_WalkValue(_walkValue);
    }
    private void MouseRotator()
    {
        mouseAxis.x = Input.GetAxis("Mouse X");
        mouseAxis.y = Input.GetAxis("Mouse Y");
        //= new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // 십자좌표계

        _rotX += (mouseAxis.x * _sensitivity);
        _rotY += (mouseAxis.y * _sensitivity);

        _rotY = Mathf.Clamp(_rotY, -50f, 50f); // 상하회전 최대값 제한
        _yTargetRotation = Quaternion.Euler(0, _rotX, 0); // 좌우회전
        _xTargetRotation = Quaternion.Euler(_rotY * 0.8f, 0, 0); // 상하회전 : 감도 변경이 필요해보임

        _targetPoint.transform.localRotation = (_yTargetRotation * _xTargetRotation) * _saveQuaternion;
        //DebugManager.ins.Log("TargetPoint.y : " + _targetPoint.transform.localEulerAngles.x);
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
        _targetPoint.transform.DOLookAt(_pinEnemy.PinTargetPoint.position, 0.1f, AxisConstraint.Y
            , null).SetEase(Ease.Linear);

        Quaternion yValue = Quaternion.Euler(0, _targetPoint.localEulerAngles.y, 0);
        _iroha.transform.localRotation = Quaternion.Slerp(_iroha.transform.localRotation, yValue, 1f);

        // MouseRotator에서 저장된 Quaternion 값이 Pin 취소되었을 때 돌아가면서 문제가 생김.
        //saveValue 
    }

    public void RollMove()
    {
        _rollDir = new Vector3(_iroha.transform.forward.x,
            _iroha.transform.position.y,
            _iroha.transform.forward.z);

        //_rollTargetPos = _iroha.transform.forward * 2.5f + this.transform.position; // 이로하 모델의 방향을 기준으로 구르도록 해야함

        //_rollMovePos = new Vector3(_rollTargetPos.x, this.transform.position.y , _rollTargetPos.z);
        //_rollDir = controller.transform.TransformDirection(_rollMovePos); // 로컬벡터를 월드벡터로 변환시킴

        /*
        vCurPos = this.transform.position;
        Vector3 vDist = vTargetPos - vCurPos;
        Vector3 vDir = vDist.normalized;
        float fDist = vDist.magnitude; // 백터를 숫자화 하여 거리를 구함
        */
        //this.transform.DOMove(_rollMovePos, 0.8f, false).SetEase(Ease.InQuad); // snapping true = 정수이동
    }
    private Vector3 _rollDir; // 구르기 방향
    private float _rollSpeed = 2f;
    private void AboutRoll()
    {
        if (animator.IsRolling) // 구르기중
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

    private Vector3 _jumpDir; // 중력 방향
    private float _gravityScale = 15f; // 중력크기

    public void Jump()
    {
        DebugManager.ins.Log("점프", DebugManager.TextColor.Blue);
        _jumpDir = controller.transform.TransformDirection(_jumpDir); // 로컬벡터를 월드벡터로 변환시킴
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
        if (animator.IsJumping) // 점프중일 때 상승하도록
        {
            //_jumpDir.y = Mathf.Lerp(_jumpDir.y, _jumpHeight, _jumpLinearValue * Time.deltaTime);
            _jumpDir.y = _jumpHeight;
        }
    }
}