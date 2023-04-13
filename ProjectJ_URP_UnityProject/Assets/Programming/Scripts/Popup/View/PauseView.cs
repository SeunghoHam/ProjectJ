using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.Popup.Base;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class PauseView : PopupSub
    {
        public FlowManager FlowManager { get; set; }
        public ResourcesManager ResourcesManager { get; set; }

        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _returnButton; // 되돌아가기

        
        // SystemButton
        [SerializeField] private Button _equipButton;
        [SerializeField] private Button _invenButton;
        [SerializeField] private Button _statusButton;
        [SerializeField] private Button _systemButton;
        
        
        // Views
        [SerializeField] private GameObject _statusView;
        private void Awake()
        {
            AddEvent();
        }
        private void AddEvent()
        {
            Debug.Log("Pause활성화");
            
            _returnButton.OnClickAsObservable().Subscribe(_ => 
                EndPause());
            _saveButton.OnClickAsObservable().Subscribe(_ =>
                DataSave());

            _statusButton.OnClickAsObservable().Subscribe(_ =>
                Action_Status());
        }
        
        #region ::: ButtonAction :::
        private void EndPause()
        {
            PopupManager.Instance.PopupList[1].GetComponent<UIPopupPause>().Hide();
        }
        private void DataSave()
        {
            DataManager.Instance.DataSave();
        }


        private void Action_Status()
        {
            _statusView.SetActive(true);
        }
        
        #endregion
    }
}