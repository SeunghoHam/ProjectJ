using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineBrain _brain;
    private void Awake()
    {
        _brain = this.GetComponent<CinemachineBrain>(); 
    }
    // 모션에 맞는거 미리 정의해두기
}
