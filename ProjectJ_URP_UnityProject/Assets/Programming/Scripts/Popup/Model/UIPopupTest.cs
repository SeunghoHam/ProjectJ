using Assets.Scripts.Manager;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common.DI;
using UnityEngine;
using Assets.Scripts.UI.Popup.PopupView;
using Assets.Scripts.MangeObject;

namespace Assets.Scripts.UI.Popup.Base
{
    public class UIPopupTest : PopupBase
    {
        [DependuncyInjection(typeof(FlowManager))]
        private FlowManager _flowManager;

        [DependuncyInjection(typeof(ResourcesManager))]
        private ResourcesManager _resourcesManager;

        [SerializeField] private TestView _testView;

        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);

            _testView.FlowManager = _flowManager;
            _testView.ResourcesManager = _resourcesManager;
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