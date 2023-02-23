using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    MouseController _mouseController;
    CharacterController controller;
    //Animator anim;
    Transform cam;
    float _speedSmoothVelocity = 1f;
    float _speedSmoothTime = 1f;
    float _currentSpeed;
    float _velocityY;
    Vector3 _moveInput;
    Vector3 _dir;

    float gravity = 25.0f;
    float moveSpeed = 3.0f;
    float rotateSpeed = 3.0f;

    [SerializeField] private GameObject _rotateActor;
    [SerializeField] private Transform _target;

    void Start()
    {
        _mouseController = this.GetComponent<MouseController>();
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
        InputAction();

        
    }
    private void LateUpdate()
    {
    }

    private void CursurSetting() // 임시니까 그냥 Movement에 박음
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, 0);
    }
    private void GetInput()
    {

        _moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized; // 이동 각도 가져오기
        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;
        forward.y = 0;
        right.y = 0;

        if (_moveInput.x == 0 && _moveInput.y == 0) // 입력이 없는 상태
        {
            _dir = Vector3.zero;
        }
        else // _dir 전달
        {
            _dir = (forward * _moveInput.y + right * _moveInput.x); // normalized 하면 1 / 0 만 존재하게 되서 딱 딱 끊김
                                                                    // 만약 공격이나 애니메이션 동작중이 아니라면 : 조건문 추가해줘야

            //Quaternion newRot = Quaternion.Euler(0, _target.transform.localRotation.y, 0); // target에서 y값만 가져오기 위해서
            _rotateActor.transform.localRotation = Quaternion.Lerp(_rotateActor.transform.localRotation, _target.transform.localRotation, 1f);
            // 이동은 target이 바라보는 방향으로 진행되도록
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);
            //Vector3 velocity = (_dir * _currentSpeed) + Vector3.up * _velocityY; // 기존 이동방식

            //controller.Move(_rotateActor.transform.localRotation* direction * Time.deltaTime);
        }
        if (_velocityY > -10) _velocityY -= Time.deltaTime * gravity;
        //Vector3 forward = cam.forward;
        //Vector3 right = cam.right;

        //forward.Normalize();
        //right.Normalize();

        // 이동 방향은 target의 RotValue를 기준으로 함
    }
    private void PlayerMovement()
    {
    


        // 애니메이션 적용 
        //anim.SetFloat("Movement", _dir.magnitude, 0.1f, Time.deltaTime);
    }
    private void PlayerRotation()
    {
        if (_dir.magnitude == 0) return;
        Vector3 rotDir = new Vector3(_dir.x, _dir.y, _dir.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotDir), Time.deltaTime);
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
