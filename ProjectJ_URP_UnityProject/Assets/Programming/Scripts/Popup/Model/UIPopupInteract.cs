using Assets.Scripts.Manager;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common.DI;
using UnityEngine;
using Assets.Scripts.UI.Popup.PopupView;

namespace Assets.Scripts.UI.Popup.Base
{
    public class UIPopupInteract : PopupBase
    {
        [DependuncyInjection(typeof(FlowManager))]
        private FlowManager _flowManager;

        [DependuncyInjection(typeof(ResourcesManager))]
        private ResourcesManager _resourcesManager;

        [SerializeField] private InteractView _basicView;

        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);

            _basicView.FlowManager = _flowManager;
            _basicView.ResourcesManager = _resourcesManager;
        }
        
        public override void UnInitialize()
        {
            base.UnInitialize();
        }
        public override void Show(params object[] data)
        {
            base.Show(data);
        }
        public override void Hide()
        {
            base.Hide();
        }
    }
}