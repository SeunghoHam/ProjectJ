using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{ 
    private CharacterController controller; // 캐릭터 움직임 관련 컴포넌트

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

    private bool _isAttacking; // 공격중인 상태에서는 움직임 막기

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
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // 이동 각도 가져오기.


        //if (direction.magnitude >= 0.1f) 
        if (direction.normalized != Vector3.zero)
        {
            // +(조건문 추가) 만약 공격이나 애니메이션 동작중이 아니라면
            if (direction.normalized.z == 1) // 정규화된 움직임(앞으로 가는중)
            {
                _rotateActor.transform.localRotation = Quaternion.Lerp(_rotateActor.transform.localRotation, _yTargetRotation, 0.3f);
            }
            else if (direction.normalized.z == -1)
            {
                // 카메라 바라보게 뒤로 돌기
            }

            if (direction.normalized.x == 1) // →
            {
            }
            else if (direction.normalized.x == -1) // ←
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
        else // 입력이 없는 상태(정규화 벡터로 받아와서 클릭 끊기면 바로 적용되게)
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
        Vector2 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // 십자좌표계
        _rotX += (mouseAxis.x * _sensitivity);
        _rotY += (mouseAxis.y * _sensitivity);

        _rotY = Mathf.Clamp(_rotY, -30f, 30f);
        _yTargetRotation = Quaternion.Euler(0, _rotX, 0); // 좌우회전
        _xTargetRotation = Quaternion.Euler(_rotY, 0, 0); // 상하회전

        _targetPoint.transform.localRotation = _yTargetRotation * _xTargetRotation; // 행렬연산이기에 곱연산이 합이된다.
    }

}
