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
    // ��ǿ� �´°� �̸� �����صα�
}
