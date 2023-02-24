using AmplifyShaderEditor;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{ 
    CharacterController controller;
    //Animator anim;
    Transform cam;
    float _speedSmoothVelocity = 1f;
    float _speedSmoothTime = 1f;
    float _currentSpeed;
    float _velocityY;

    float gravity = 25.0f;
    float moveSpeed = 5.0f;
    float rotateSpeed = 3.0f;

    [SerializeField] private GameObject _rotateActor;
    [SerializeField] private Transform _target;

    private float _rotX;
    private float _rotY;
    private float _sensitivity = 5f;

    Quaternion _xTargetRotation;
    Quaternion _yTargetRotation;
    void Start()
    {
        controller = this.gameObject.GetComponent<CharacterController>();
        cam = Camera.main.transform;
        CursurSetting();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        //PlayerMovement();
        //PlayerRotation();
        MouseRotator();
        InputAction();

        
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
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // 이동 각도 가져오기. normalize 해제하기
        /*
        if (direction.x == 0 && direction.z == 0) // 입력이 없는 상태
        {
            direction = Vector3.zero;
        }
        else // 방향 전달*/
        {
            // +(조건문 추가) 만약 공격이나 애니메이션 동작중이 아니라면
            if (direction.normalized.z == 1) // 정규화된 움직임(앞으로 가는중)
            {
                DebugManager.ins.Log("↑", DebugManager.TextColor.Yellow);
                _rotateActor.transform.localRotation = Quaternion.Lerp(_rotateActor.transform.localRotation, _yTargetRotation, 0.3f);
            }
            else if (direction.normalized.z == -1)
            {
                // 카메라 바라보게 뒤로 돌기
                DebugManager.ins.Log("↓", DebugManager.TextColor.Yellow);
            }

            if (direction.normalized.x == 1)
            {
                DebugManager.ins.Log("→", DebugManager.TextColor.Blue);
            }
            else if (direction.normalized.x == -1)
            {
                DebugManager.ins.Log("←", DebugManager.TextColor.Blue);
                
            }

            // _dir = (forward * _moveInput.y + right * _moveInput.x); // 정규화(normalize) 하면 1 / 0 만 존재하게 되서 딱 딱 끊김
            // Quaternion을 전달해야 회전이 깔끔하게 됨(EulerAngles불가)
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);
            controller.Move(_rotateActor.transform.localRotation * direction * _currentSpeed * Time.deltaTime);
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

        _target.transform.localRotation = _yTargetRotation * _xTargetRotation; // 행렬연산이기에 곱연산이 합이된다.
    }
    private void InputAction()
    {
        // 근처에 적이 없다면 타겟팅을 하지 않는다.
        if (Input.GetKeyDown(KeyCode.Q))
        {
             DebugManager.ins.Log("타겟팅 동작", DebugManager.TextColor.Blue);
        }


    }
}
