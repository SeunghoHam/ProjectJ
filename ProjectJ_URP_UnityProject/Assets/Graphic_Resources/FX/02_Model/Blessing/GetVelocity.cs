using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class GetVelocity : MonoBehaviour
{

    public Vector3 oldPosition;
    public VisualEffect Blessing_Tail;

    // Update is called once per frame
    void Update()
    {
        Vector3 Trail_velocity = this.transform.position - oldPosition;
        if (Blessing_Tail != null) Blessing_Tail.SetVector3("Trail_Velocity", Trail_velocity);
        oldPosition = this.transform.position;
    }
}
