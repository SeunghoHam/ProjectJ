using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineBrain _brain;

    [Space(5)]
    [Header("VirtualCamera")]
    [SerializeField] private CinemachineVirtualCamera _cam3rdSight;
    [SerializeField] private CinemachineVirtualCamera _cam3rdSight2;
    
    
    [Space(5)]
    [Header("offset")]
    private Cinemachine3rdPersonFollow _cameraComponent;
    public Vector3 _shoulderOffset;
    private Vector3 _damping;

    public float _targetPoint_xValue;

    private Vector3[] offsetDefinition = // ������ �����صα�
    {
        //new Vector3(0,-0.2f,0.4), // ���� ����
        new Vector3(0,0,0) // �� ����
    };

    private void Awake()
    {
        StartCoroutine(CameraSettingRoutine());
    }

    private IEnumerator CameraSettingRoutine()
    {
        yield return new WaitUntil(() => Character.Instance != null);
        Character.Instance.GetCameraSystem = this;
        _cameraComponent = _cam3rdSight.GetCinemachineComponent<Cinemachine3rdPersonFollow>(); // ShouldefOffset ������ ���ؼ� ������Ʈ �޾ƿ���

        //_cameraComponent = _cam3rdSight.GetComponent<Cinemachine3rdPersonFollow>();
        _cam3rdSight.Follow = Character.Instance.TargetPoint;
        _cam3rdSight.LookAt = Character.Instance.TargetPoint;
    }
    
    // ShouldOffset ���ǽ�Ű��
    public void ChangeOffset(Vector3 offsetValue)
    {
        _cameraComponent.ShoulderOffset = offsetValue;
    }

    // CameraDamping �����Ű��
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
