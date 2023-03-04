using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineBrain _brain;

    [Space(5)]
    [SerializeField] private CinemachineVirtualCamera _cam3rdSight;
    private void Awake()
    {
        StartCoroutine(CameraSettingRoutine());
    }

    private IEnumerator CameraSettingRoutine()
    {
        yield return new WaitUntil(() => Character.Instance != null);

        _cam3rdSight.Follow = Character.Instance.TargetPoint;
        _cam3rdSight.LookAt = Character.Instance.TargetPoint;
    }
}
