using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Assets.Scripts.Manager;
using Assets.Scripts.UI;


public class CanvasInputManager : MonoBehaviour
{
    private enum CurrentActivePopup
    {
        NONE,
        PAUSE,
    }
    private CurrentActivePopup activePopup;

    private void Awake()
    {
        InputSetting();
        activePopup = CurrentActivePopup.NONE;
    }
   
    private void InputSetting()
    {
        //var Esc = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.Escape)).Subscribe(_=> Input_ESC()).AddTo(gameObject);
        var p = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.P)).Subscribe(_ => Input_I()).AddTo(gameObject);
    }
    private void Input_ESC() // �Ͻ����� Ȱ��ȭ
    {
        if(activePopup == CurrentActivePopup.PAUSE)
        {
               
        }
        else
        { 
            Debug.Log("�˾� ����1");
            FlowManager.Instance.AddSubPopup(PopupStyle.Pause);
            activePopup = CurrentActivePopup.PAUSE;
            Debug.Log("�˾� ����2");
        }

        // ���� Ȱ��ȭ �Ǿ��ִ� �˾��� �ִٸ� ��Ȱ��ȭ/
        // ���ٸ� �Ͻ����� Ȱ��ȭ

    }
    private void Input_I() // �κ��丮 Ȱ��ȭ
    {
        FlowManager.Instance.AddSubPopup(PopupStyle.Pause);
        // �κ��丮�� Ȱ��ȭ �Ǿ��ִٸ� �κ��丮 �ݱ�.
        // �ƴ϶�� �κ��丮 Ȱ��ȭ
    }
}
