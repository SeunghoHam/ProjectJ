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
        
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, 0);
    }
    private void Init()
    {
        DependuncyInjection.Inject(this);
        ManageObjectFacade.Initialize();
        FlowManager.Instance.AddSubPopup(PopupStyle.Basic);
    }
}