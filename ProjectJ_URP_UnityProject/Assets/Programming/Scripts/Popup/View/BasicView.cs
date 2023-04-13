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

        [SerializeField] GameObject _interactObject; // 상호작용 가능 할 때 활성화시킬 오브젝트
        [SerializeField] GameObject _deadObject; // Dead 오브젝트



        public enum CurrentViewType
        {
            None,
            Status,
            System,
        }

        public CurrentViewType _currentViewType;
        
        // 팝업 활성화여부
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
            
               _pauseReactive = new ReactiveProperty<bool>(false);
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
            // 캐릭터가 축복 범위에 있단 것을 반환
            if (!Character.Instance.CanInteract ||
                Character.Instance.Animator.AnimState != CharacterAnimator.ChaAnimState.Idle)
                return;
            
            // :After SeatMotion (앉는 모션 뒤에 실행되도록)
            if (!_interactActive)
            {
                // 활성화
                PopupActive(false);
                FlowManager.AddSubPopup(PopupStyle.Interact);
                StartCoroutine(InteractSettingRoutine());
                Character.Instance.IsInteract = true;
                BattleManager.Instance.CusrorVisible(true);
            }
            else
            {
                // 비활성화
                PopupActive(true);
                popup_Interact.Hide();
                popup_Interact = null;
                _interactActive = false;
                Character.Instance.IsInteract = false;
                BattleManager.Instance.CusrorVisible(false);
            }
        }


        private ReactiveProperty<bool> _pauseReactive;  // 한번만 실행되게 만들기
        // 재입력 되었을때도 사라지게 해야함
        public void Input_Pause()
        {
            if (_currentViewType == CurrentViewType.None)
            {
                DebugManager.ins.Log("_currentViewTyep = " + _currentViewType.ToString());
                PopupActive(false); // E 키 누르라는 유도오브젝트 활성여부
                FlowManager.AddSubPopup(PopupStyle.Pause);
                StartCoroutine(PauseSettingRoutine());
                Character.Instance.IsInteract = true;
                BattleManager.Instance.CusrorVisible(true);
                _currentViewType = CurrentViewType.Status;
            }
            else
            {
                DebugManager.ins.Log("_currentViewType = " + _currentViewType.ToString());
                if (popup_Pause != null)
                    popup_Pause.Hide();
                if (popup_Interact != null)
                    popup_Interact.Hide();

                //yield return new WaitUntil(() => PopupManager.PopupList.Count == 1);
                BattleManager.Instance.CusrorVisible(false);
                Character.Instance.IsInteract = false;
                _currentViewType = CurrentViewType.None;
            }

        }

        private IEnumerator InteractSettingRoutine()
        {
            yield return new WaitUntil(() => PopupManager.PopupList.Count > 1); // 1 : basic , 2 : 추가되는 팝업
            popup_Interact = PopupManager.PopupList[1].GetComponent<UIPopupInteract>();
            _interactActive = true;
        }

        private IEnumerator PauseSettingRoutine()
        {
            yield return new WaitUntil(() => PopupManager.PopupList.Count > 1); // 1 : basic , 2 : 추가되는 팝업
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
            // fillAmount의 최대값은 1 이니까 0.n 의 값이 나오도록 해야함
            //_hpValue.fillAmount = (float)Character.Instance.CurHP / (float)Character.Instance.MaxHP; // 최대값이 1
            _hpValue.DOFillAmount((float)Character.Instance.CurHP / (float)Character.Instance.MaxHP, 0.2f);
        }
        public void GetPotion()
        {
            DebugManager.ins.Log("물약 먹음", DebugManager.TextColor.Blue);
            Character.Instance.Animator.Anim_Drinking();
        }

    }
}