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

        [SerializeField] GameObject _interactObject; // ��ȣ�ۿ� ���� �� �� Ȱ��ȭ��ų ������Ʈ

        private void Start()
        {
            Init();
            AddEvent();
            DependuncyInjection.Inject(this);
        }
        private void Init()
        {
            _interactObject.SetActive(false);
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

            var drinkStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.R)).Subscribe(_ => GetPotion()); // ����
        }

        private bool _interactActive = false;
        private void Interact()
        {
            // ĳ���Ͱ� �ູ ������ �ִ� ���� ��ȯ
            if (!Character.Instance.CanInteract)
                return;

            if (!_interactActive)
            {
                // Ȱ��ȭ
                FlowManager.AddSubPopup(PopupStyle.Interact);
                StartCoroutine(InteractSettingRoutine());
                Character.Instance.IsInteract = true;
                BattleManager.Instance.CusrorVisible(true);
            }
            else
            {
                // ��Ȱ��ȭ
                popup_Interact.Hide();
                popup_Interact = null;
                _interactActive = false;
                Character.Instance.IsInteract = false;
                BattleManager.Instance.CusrorVisible(false);
            }
        }
        private IEnumerator InteractSettingRoutine()
        {
            yield return new WaitUntil(() => PopupManager.PopupList.Count > 1); // 1 : basic , 2 : �߰��Ǵ� �˾�
            popup_Interact = PopupManager.PopupList[1].GetComponent<UIPopupInteract>();
            _interactActive = true;
        }

        public void IntearctActive(bool ison)
        {
            _interactObject.SetActive(ison);
        }
        private void HpValueChange()
        {
            // fillAmount�� �ִ밪�� 1 �̴ϱ� 0.n �� ���� �������� �ؾ���
            //_hpValue.fillAmount = (float)Character.Instance.CurHP / (float)Character.Instance.MaxHP; // �ִ밪�� 1
            _hpValue.DOFillAmount((float)Character.Instance.CurHP / (float)Character.Instance.MaxHP, 0.2f);
        }
        private void GetPotion()
        {
            DebugManager.ins.Log("���� ����", DebugManager.TextColor.Blue);
        }

    }
}