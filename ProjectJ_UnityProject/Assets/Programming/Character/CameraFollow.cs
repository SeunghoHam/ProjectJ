using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector2 clampAxis = new Vector2(60, 60); // clamp

    float followSmoothing = 5f;
    float rotateSmoothing = 5f;
    float sensitivity = 60f;

    float rotX;
    float roty;
    bool cursorLocked = false;

    Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, followSmoothing * Time.deltaTime);

        CameraTargetRotation();
    }

    private void CameraTargetRotation()
    {
        
    }
}
