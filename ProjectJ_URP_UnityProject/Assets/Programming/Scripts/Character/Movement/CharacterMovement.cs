using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Timeline;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController controller; // AboutCharacterMovement
    private CharacterAnimator animator;
    private CameraSystem cameraSystem;

    float _speedSmoothVelocity = 0.5f; // default 1
    float _speedSmoothTime = 1f;
    float _currentSpeed = 2.5f;

    float moveSpeed = 5.0f; // 5
    float rotateSpeed = 10.0f;

    // Movement
    [SerializeField] private GameObject _iroha; // irohaModel + Get AnimaotrActor

    [SerializeField] private Transform _targetPoint;

    private Vector3 direction; // Input KeyDirection controller.Move를 위해서 3차원 벡터여야함

    private Vector2 mouseAxis;
    [SerializeField] private float _rotX;
    [SerializeField] private float _rotY;

    // AfterPinTarget Release, SaveValue
    [SerializeField] private float setRotX;
    [SerializeField] private float setRotY;
    private float _sensitivity = 5f;
    private Quaternion _xTargetRotation;
    private Quaternion _yTargetRotation;

    // Attack
    private bool _isAttacking;

    // PinTarget
    private bool _isPin;
    private Enemy _pinEnemy;

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
                    _isPin = value; // 
                }
                else
                {
                    Debug.Log("SaveValue :" + setRotX + ", " + setRotY);
                    _isPin = value;
                    _rotX = setRotX;
                    _rotY = setRotY;
                    Debug.Log("ChangeValue :" + _rotX + ", " + _rotY);
                }
            }
        }
    }

    void Start()
    {
        controller = this.gameObject.GetComponent<CharacterController>();

        animator = Character.Instance.Animator;
        cameraSystem = Character.Instance.cameraSystem;

        controller
            .ObserveEveryValueChanged(x => x.isGrounded)
            .ThrottleFrame(5)
            .Subscribe(x => _isGrounded = x);
    }

    bool _isGrounded; // 좀 더 정밀도 높은 isGrounded

    void FixedUpdate()
    {
        // if Interacting = return
        if (!Character.Instance.IsInteract)
        {
            GetInput();
            Movement();
            WalkBlend();

            if (!_isPin)
                MouseRotator();
            else
                PinRotator();

            AboutJump();
            AboutRoll();
            cameraSystem._targetPoint_xValue = _targetPoint.localRotation.x;
        }
        //cameraSystem._targetPoint_xValue = _xTargetRotation.x;
    }

    private bool isFront;
    private bool isSide;
    private void GetInput()
    {
        if (animator.AnimState == CharacterAnimator.ChaAnimState.Roll ||
            animator.AnimState == CharacterAnimator.ChaAnimState.Attack)
        {
            _currentSpeed = 0f;
            return;
        }

        _currentSpeed = 2.5f;
        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // y 값에 현재 위치 할당해보기
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // 어색하면 Raw빼기
        if (input != Vector2.zero)
        {
            if (!_isPin) // Pin [ x ]
            {
                //Debug.Log("input : " + input);
                //DebugManager.ins.Log("iroha Model Rot" + _iroha.transform.localEulerAngles);
                // 키를 누른 방향으로 _iroha의 회전 변경되도록 해야함 이로하만 y 회전 +90
                
                Quaternion horizontal = Quaternion.identity;
                Quaternion vertical = Quaternion.identity;

                if (direction.z != 0)
                {
                    //Debug.Log("Front이동");
                    isFront = true;
                }
                else isFront = false;

                if (direction.x != 0)
                {
                    isSide = true;
                    //Debug.Log("Side 이동");
                }
                else isSide = false;
                
                if (isFront)
                {
                    if (direction.z < 0) // 뒤로 갈때는 Identity가 아닌 설정값으로 대체
                        vertical = Quaternion.Euler(0, direction.normalized.z * 180f, 0); // Front : Back
                }
                
                if (isSide)
                {
                    horizontal = Quaternion.Euler(0, direction.normalized.x * 90f, 0); // Left : Right
                }

                Quaternion target =
                    Quaternion.Euler(0, _targetPoint.transform.localEulerAngles.y, 0); // 현재 바라보고있는 시점을 기준으로

                _iroha.transform.localRotation = Quaternion.Slerp(_iroha.transform.localRotation,
                    target * horizontal * vertical, rotateSpeed * Time.deltaTime);

                //_iroha.transform.localRotation = target * horizontal * vertical;

                // 움직일때는 TargetPoint 의 x 회전값은 무시하고 y 회전값만 적용되도록 해야함.
                // 왜 why x의 회전값이 바뀌면 캐릭터 이동이 하늘로 뜸                
            }
        }

        // 입력이 없는 상태(정규화 벡터로 받아와서 클릭 끊기면 바로 적용되게) else
    }

    private void Movement()
    {
        if (animator.CanMove())
        {
            controller.Move(_yTargetRotation // (targetPoint) RotationValue
                            * direction // iroha Model LookDirection
                            * _currentSpeed
                            * Time.deltaTime);
        }

        // 점프 정의
        controller.Move(_jumpDir * Time.deltaTime);

        // 애니메이션 관련
        //_walkValue = Mathf.Abs(direction.x + direction.z); // 어떤 방향이든 입력이 있다면 0 초과로 나옴
    }

    private void MouseRotator()
    {
        mouseAxis.x = Input.GetAxis("Mouse X");
        mouseAxis.y = Input.GetAxis("Mouse Y");
        //= new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // 십자좌표계

        _rotX += (mouseAxis.x * _sensitivity);
        if (_rotX >= 360f)
            _rotX = 0f;
        else if (_rotX <= -360f)
            _rotX = 0f;

        _rotY += (mouseAxis.y * _sensitivity);

        _rotY = Mathf.Clamp(_rotY, -50f, 50f); // 상하회전 최대값 제한
        _yTargetRotation = Quaternion.Euler(0, _rotX, 0); // 좌우회전
        _xTargetRotation = Quaternion.Euler(_rotY, 0, 0); // 상하회전 : 감도 변경이 필요해보임

        _targetPoint.transform.localRotation = (_yTargetRotation * _xTargetRotation);
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
        setRotX = _targetPoint.localEulerAngles.y;
        setRotY = _targetPoint.localEulerAngles.x;
        _targetPoint.transform.DOLookAt(_pinEnemy.PinTargetPoint.position, 0.1f, AxisConstraint.Y
            , null).SetEase(Ease.Linear);

        Quaternion yValue = Quaternion.Euler(0, _targetPoint.localEulerAngles.y, 0);
        _iroha.transform.localRotation = yValue;
        //Quaternion.Slerp(_iroha.transform.localRotation, yValue, 1f);

        // new : yTargetRotation이 MouseRotator()에서만 변경되서 그랬음
        _yTargetRotation = _iroha.transform.localRotation;
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
    private float _rollSpeed = 3f;

    private void AboutRoll()
    {
        if (!animator.IsRolling)
            return;
        //controller.Move(_rollDir * Time.deltaTime);
        controller.Move(_rollDir * _rollSpeed * Time.deltaTime);
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

    private void AboutJump()
    {
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

    // [-1:Back, 0:idle, 1:Front]
    private void WalkBlend()
    {
        animator.Anim_GetDirection(direction.x, direction.z);

        float walkBlend;
        if (direction.z < 0) // 뒤로걷기
        {
            walkBlend = direction.z;
        }
        else
        {
            walkBlend =
                Mathf.Abs(direction.x) + direction.z;
        }

        animator.Blend_Walk(Mathf.Clamp(walkBlend, -1, 1));
    }
}