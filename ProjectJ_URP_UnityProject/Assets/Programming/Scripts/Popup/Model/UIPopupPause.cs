using Assets.Scripts.Common.DI;
using Assets.Scripts.Manager;
using Assets.Scripts.UI.Popup.PopupView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Popup.Base
{
    public class UIPopupPause : PopupBase
    {
        [DependuncyInjection(typeof(FlowManager))]
        private FlowManager _flowManager;

        [DependuncyInjection(typeof(ResourcesManager))]
        private ResourcesManager _resourcesManager;

        [SerializeField] private PauseView _pauseView;

        public override void Initialize()   
        {
            base.Initialize();
            DependuncyInjection.Inject(this);

            _pauseView.FlowManager = _flowManager;
            _pauseView.ResourcesManager = _resourcesManager;
        }
        public override void Hide()
        {
            base.Hide();
            //if(PopupManager.Instance.PopupList[0].GetComponent<UIPopupBasic>()._basicView.popup_Pause != null)
                //PopupManager.Instance.PopupList[0].GetComponent<UIPopupBasic>()._basicView.Input_Pause();
        }
        
        public override void UnInitialize()
        {
            base.UnInitialize();
        }
        public override void Show(params object[] data)
        {
            base.Show(data);
        }
        
    }
}