using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.Popup.Base;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class PauseView : PopupSub
    {
        public FlowManager FlowManager { get; set; }
        public ResourcesManager ResourcesManager { get; set; }
        public DataManager DataManager { get; set; }
        [SerializeField] private Button _closeButton; // 되돌아가기

        
        // SystemButton
        [Header("SystemButton")]
        [SerializeField] private Button _equipButton;
        [SerializeField] private Button _invenButton;
        [SerializeField] private Button _statusButton;
        [SerializeField] private Button _systemButton;
        
        
        // Views
        [Space(10)]
        [Header("View")]
        [SerializeField] private GameObject _statusView;
        [SerializeField] private GameObject _systemView;
        private void Awake()
        {
            AddEvent();
            _statusView.SetActive(false);
            _systemView.SetActive(false);
        }
        private void AddEvent()
        {
            //Debug.Log("Pause활성화");
            _closeButton.OnClickAsObservable().Subscribe(_ => 
                Action_Close());
            _statusButton.OnClickAsObservable().Subscribe(_ =>
                Action_Status());
            _systemButton.OnClickAsObservable().Subscribe(_ =>
                Action_System());
        }
        
        #region ::: ButtonAction :::
        private void Action_Close()
        {
            // 활성화 되어있는 View가있음
            if (_statusView.activeSelf || _systemView.activeSelf)
            {
                _statusView.SetActive(false);
                _systemView.SetActive(false);
            }
            else 
            {
                //Debug.Log("PauseView 에서 호출");
                //PopupManager.Instance.PopupList[1].GetComponent<UIPopupPause>().Hide();
                PopupManager.Instance.PopupList[0].GetComponent<UIPopupBasic>()._basicView.Input_Pause();
            }
        }

        private void Action_Status()
        {
            PopupManager.Instance.PopupList[0].GetComponent<UIPopupBasic>()._basicView._currentViewType =
                BasicView.CurrentViewType.Status;
            _statusView.SetActive(true);
            _statusView.GetComponent<ViewStatus>().GetDataManager(DataManager);
        }

        private void Action_System()
        {
            PopupManager.Instance.PopupList[0].GetComponent<UIPopupBasic>()._basicView._currentViewType =
                BasicView.CurrentViewType.System;
            _systemView.SetActive(true);
        }
        
        #endregion
    }
}