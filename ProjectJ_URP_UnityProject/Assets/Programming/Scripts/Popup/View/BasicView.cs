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
        public UIPopupPause popup_Pause;

        [SerializeField] Image _hpValue;
        [SerializeField] Image _mpValue;

        [SerializeField] GameObject _interactObject; // ��ȣ�ۿ� ���� �� �� Ȱ��ȭ��ų ������Ʈ
        [SerializeField] GameObject _deadObject; // Dead ������Ʈ


        // �˾� Ȱ��ȭ����
        private bool _interactActive = false;
        private bool _pauseActive = false;

        private void Start()
        {
            Init();
            AddEvent();
            DependuncyInjection.Inject(this);
        }
        private void Init()
        {
            _interactObject.SetActive(false);
            _deadObject.SetActive(false);
        }
        private void AddEvent()
        {
            this.ObserveEveryValueChanged(_ => Character.Instance.CurHP)
                //.Where(_ => )
                .Subscribe(_ => HpValueChange())
                .AddTo(gameObject);
            
        }


        public void Input_Interact()
        {
            // ĳ���Ͱ� �ູ ������ �ִ� ���� ��ȯ
            if (!Character.Instance.CanInteract ||
                Character.Instance.Animator.AnimState != CharacterAnimator.ChaAnimState.Idle)
                return;
            
            // :After SeatMotion (�ɴ� ��� �ڿ� ����ǵ���)
            if (!_interactActive)
            {
                // Ȱ��ȭ
                PopupActive(false);
                FlowManager.AddSubPopup(PopupStyle.Interact);
                StartCoroutine(InteractSettingRoutine());
                Character.Instance.IsInteract = true;
                BattleManager.Instance.CusrorVisible(true);
            }
            else
            {
                // ��Ȱ��ȭ
                PopupActive(true);
                popup_Interact.Hide();
                popup_Interact = null;
                _interactActive = false;
                Character.Instance.IsInteract = false;
                BattleManager.Instance.CusrorVisible(false);
            }
        }

        public void Input_Pause()
        {
            if(!_pauseActive)
            {
                Debug.Log("Pause Ȱ��ȭ");
                PopupActive(false);
                FlowManager.AddSubPopup(PopupStyle.Pause);
                StartCoroutine(PauseSettingRoutine());
                Character.Instance.IsInteract = true;
                BattleManager.Instance.CusrorVisible(true);
            }
            else
            {
                Debug.Log("Pause �����ϱ�");
                //popup_Pause.Hide();
                popup_Pause = null;
                _pauseActive = false;
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

        private IEnumerator PauseSettingRoutine()
        {
            yield return new WaitUntil(() => PopupManager.PopupList.Count > 1); // 1 : basic , 2 : �߰��Ǵ� �˾�
            popup_Pause = PopupManager.PopupList[1].GetComponent<UIPopupPause>();
            _pauseActive = true;
        }

        

        
        /// <summary>
        /// InteractObject Enable ( ex) Press "E" )
        /// </summary>
        /// <param name="ison"></param>
        public void PopupActive(bool ison)
        {
            _interactObject.SetActive(ison);
        }
        public void DeadActive(bool ison)
        {
            _deadObject.SetActive(ison);
        }
        private void HpValueChange()
        {
            // fillAmount�� �ִ밪�� 1 �̴ϱ� 0.n �� ���� �������� �ؾ���
            //_hpValue.fillAmount = (float)Character.Instance.CurHP / (float)Character.Instance.MaxHP; // �ִ밪�� 1
            _hpValue.DOFillAmount((float)Character.Instance.CurHP / (float)Character.Instance.MaxHP, 0.2f);
        }
        public void GetPotion()
        {
            DebugManager.ins.Log("���� ����", DebugManager.TextColor.Blue);
            Character.Instance.Animator.Anim_Drinking();
        }

    }
}