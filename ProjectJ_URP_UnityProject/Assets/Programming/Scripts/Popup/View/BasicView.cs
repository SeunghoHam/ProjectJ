using Assets.Scripts.Common.DI;
using Assets.Scripts.Manager;
using Assets.Scripts.MangeObject;
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
                //Debug.Log("E");
                FlowManager.AddSubPopup(PopupStyle.Interact);
            }
            ).AddTo(gameObject);
        }

        private void HpValueChange()
        {
            // fillAmount�� �ִ밪�� 1 �̴ϱ� 0.n �� ���� �������� �ؾ���
            //_hpValue.fillAmount = (float)Character.Instance.CurHP / (float)Character.Instance.MaxHP; // �ִ밪�� 1
            _hpValue.DOFillAmount((float)Character.Instance.CurHP / (float)Character.Instance.MaxHP, 0.2f);
        }

    }
}