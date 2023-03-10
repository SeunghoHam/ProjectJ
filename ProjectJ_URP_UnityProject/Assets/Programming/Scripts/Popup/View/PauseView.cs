using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class PauseView : MonoBehaviour
    {
        public FlowManager FlowManager { get; set; }
        public ResourcesManager ResourcesManager { get; set; }

        private void Start()
        {
            AddEvent();
        }
        private void AddEvent()
        {

        }
    }
}