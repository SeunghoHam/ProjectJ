using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineBrain _brain;

    [Space(5)]
    [SerializeField] private CinemachineVirtualCamera _cam3rdSight;

    [SerializeField] private Cinemachine3rdPersonFollow _cameraComponent;
    public Vector3 _shoulderOffset;
    private Vector3 _damping;

    public float _targetPoint_xValue;

    private Vector3[] offsetDefinition = // 오프셋 정의해두기
    {
        //new Vector3(0,-0.2f,0.4), // 보통 상태
        new Vector3(0,0,0) // 위 상태
    };

    private void Awake()
    {
        StartCoroutine(CameraSettingRoutine());
    }

    private IEnumerator CameraSettingRoutine()
    {
        yield return new WaitUntil(() => Character.Instance != null);
        Character.Instance.GetCameraSystem = this;
        _cameraComponent = _cam3rdSight.GetCinemachineComponent<Cinemachine3rdPersonFollow>(); // ShouldefOffset 변경을 위해서 컴포넌트 받아오기

        //_cameraComponent = _cam3rdSight.GetComponent<Cinemachine3rdPersonFollow>();
        _cam3rdSight.Follow = Character.Instance.TargetPoint;
        _cam3rdSight.LookAt = Character.Instance.TargetPoint;
    }


    // ShouldOffset 정의시키기
    public void ChangeOffset(Vector3 offsetValue)
    {
        _cameraComponent.ShoulderOffset = offsetValue;
    }

    // CameraDamping 적용시키기
    public void ChangeDamping()
    {
        _cameraComponent.Damping = _damping;
    }

    private void Update()
    {
        /*
        _cameraComponent.ShoulderOffset = new Vector3(0,
            -0.2f,
            //Mathf.Abs(0.4f - (_targetPoint_xValue * 0.1f)));
            0.4f - Mathf.Abs(_targetPoint_xValue * 0.1f));*/
    }

}
