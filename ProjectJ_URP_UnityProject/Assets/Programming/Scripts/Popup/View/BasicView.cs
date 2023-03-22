using Assets.Scripts.Common.DI;
using Assets.Scripts.Manager;
using Assets.Scripts.MangeObject;
using Assets.Scripts.UI.Popup.Base;
using Assets.Scripts.Util;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class BasicView : MonoBehaviour
    {
        public FlowManager FlowManager { get; set; }
        public ResourcesManager ResourcesManager { get; set; }
        public PopupManager PopupManager { get; set; }

        public UIPopupInteract popup_Interact;
       
        [SerializeField] Image _hpValue;
        [SerializeField] Image _mpValue;

        private void Start()
        {
            AddEvent();
            DependuncyInjection.Inject(this);
        }
        private void AddEvent()
        {
            this.ObserveEveryValueChanged(_ => Character.Instance.CurHP)
                //.Where(_ => )
                .Subscribe(_ => HpValueChange())
                .AddTo(gameObject);

            this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.E)).Subscribe(_ =>
            {
                Interact();
            }
            ).AddTo(gameObject);
        }

        private bool _interactActive = false;
        private void Interact()
        {
            if(!_interactActive)
            {
                FlowManager.AddSubPopup(PopupStyle.Interact);
                StartCoroutine(InteractSettingRoutine());
                //BattleManager.Instance.CursorVisible(true);
            }
            else
            {
                popup_Interact.Hide();
                popup_Interact = null;
                _interactActive = false;
                //BattleManager.Instance.CursorVisible(false);
            }
        }
        private IEnumerator InteractSettingRoutine()
        {
            yield return new WaitUntil(() => PopupManager.PopupList.Count > 1); // 1 : basic , 2 : 추가되는 팝업
            popup_Interact = PopupManager.PopupList[1].GetComponent<UIPopupInteract>();
            _interactActive = true;
        }
        private void HpValueChange()
        {
            // fillAmount의 최대값은 1 이니까 0.n 의 값이 나오도록 해야함
            //_hpValue.fillAmount = (float)Character.Instance.CurHP / (float)Character.Instance.MaxHP; // 최대값이 1
            _hpValue.DOFillAmount((float)Character.Instance.CurHP / (float)Character.Instance.MaxHP, 0.2f);
        }

    }
}