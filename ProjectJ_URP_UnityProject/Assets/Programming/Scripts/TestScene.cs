using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Common.DI;
using Assets.Scripts.Manager;
using Assets.Scripts.UI;
using Assets.Scripts.MangeObject;

public class TestScene : MonoBehaviour
{
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        DependuncyInjection.Inject(this);
        ManageObjectFacade.Initialize();
        FlowManager.Instance.AddSubPopup(PopupStyle.Test);
    }
}