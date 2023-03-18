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
    private void Input_ESC() // 일시정지 활성화
    {
        if(activePopup == CurrentActivePopup.PAUSE)
        {
               
        }
        else
        { 
            Debug.Log("팝업 생성1");
            FlowManager.Instance.AddSubPopup(PopupStyle.Pause);
            activePopup = CurrentActivePopup.PAUSE;
            Debug.Log("팝업 생성2");
        }

        // 만약 활성화 되어있는 팝업이 있다면 비활성화/
        // 없다면 일시정지 활성화

    }
    private void Input_I() // 인벤토리 활성화
    {
        FlowManager.Instance.AddSubPopup(PopupStyle.Pause);
        // 인벤토리가 활성화 되어있다면 인벤토리 닫기.
        // 아니라면 인벤토리 활성화
    }
}
