using Assets.Scripts.Common.DI;
using Assets.Scripts.Manager;
using Assets.Scripts.MangeObject;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class InteractView : PopupSub
    {
        public FlowManager FlowManager { get; set; }
        public ResourcesManager ResourcesManager { get; set; }

        [SerializeField] private Button _restButton;

        private void Awake()
        {
            AddEvent();
        }
        private void AddEvent()
        {
            //DebugManager.ins.Log("InteractView 활성화",color);
            _restButton.OnClickAsObservable().Subscribe(_ =>
            {
                Rest();
            });
        }
        private void Rest() 
        {
            DebugManager.ins.Log("쉬기", DebugManager.TextColor.Yellow);
        }

    }
}