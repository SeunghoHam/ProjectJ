using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniRx;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterMovement : MonoBehaviour
{
   
    CharacterController controller;
    Animator anim;
    Transform cam;
    float _speedSmoothVelocity;
    float _speedSmoothTime;
    float _currentSpeed;
    float _velocityY;
    Vector3 _moveInput;
    Vector3 _dir;

    float gravity = 25.0f;
    float moveSpeed = 2.0f;
    float rotateSpeed = 3.0f;

    [SerializeField] private GameObject _rotateActor;
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
        PlayerMovement();
        //PlayerRotation();
        InputAction();
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

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        _dir = (forward * _moveInput.y + right * _moveInput.x).normalized;
    }
    private void PlayerMovement()
    {
        _currentSpeed = Mathf.SmoothDamp(_currentSpeed, moveSpeed, ref _speedSmoothVelocity, _speedSmoothTime * Time.deltaTime);

        if (_velocityY > -10) _velocityY -= Time.deltaTime * gravity;

        Vector3 velocity = (_dir * _currentSpeed) + Vector3.up * _velocityY;
        controller.Move(velocity * Time.deltaTime);


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
