using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Common.DI;
public class TestScene : MonoBehaviour
{
    private void Start()
    {
        DependuncyInjection.Inject(this);


    }
}
