using System;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
using System.Collections;
using Assets.Scripts.Manager;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Popup.Base;

public class InputManager : MonoBehaviour
{
    private CharacterMovement Movement;
    private CharacterAnimator Animator;

    private WeaponController weaponController;

    // Input
    private float _refChargeTime = 0.7f;
    private float _curChargeTime;

    private bool _isCharge; // 차지공격(좌클릭 꾹)
    private bool _isReady; // 우클릭시 준비상태

    private bool _isContinuous; // 연속공격을 하면 되는지

    private UIPopupBasic popup_Basic; // BasicView에서 동작시킬것들 가져옴

    private void Start()
    {
        Animator = Character.Instance.Animator;
        Movement = Character.Instance.Movement;
        weaponController = Character.Instance.weaponController;

        InputSetting();
    }

    private void InputSetting()
    {
        var mouseLeftDownStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ => Attack());
        var mouseLeftUpStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonUp(0));

        var mouseRightDownStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(1));
        var mouseRightUpStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonUp(1));

        var targetPinStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.Q))
            .Subscribe(_ => PinTarget());
        var jumpStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.F)).Subscribe(_ => Jump());
        var rollStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.Space)).Subscribe(_ => Roll());
        var runStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.LeftShift)).Subscribe(_ => Run());
        var runCancelStrema = this.UpdateAsObservable().Where(_ => Input.GetKeyUp(KeyCode.LeftShift))
            .Subscribe(_ => RunCancel());

        // 물약 회복 -> BasicView로 이동
        // Interact활성화 -> BasicView로 이동
        var potionStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.R))
            .Subscribe(_ => GetPotion());
        var interactStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.E))
            .Subscribe(_ => Interact());
        var pauseStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.P)).Subscribe(_ => Pause());

        /* [About] LongClick
        mouseLeftDownStream
            // 마우스 클릭되면 2초후 OnNext 실행
            .SelectMany(_ => Observable.Timer(TimeSpan.FromSeconds(_refChargeTime))) // _chargeTime만큼 모아서 때리면 강공격
            .TakeUntil(mouseLeftUpStream) // 도중에 mouseUp 되면 스트림 초기화
            .RepeatUntilDestroy(gameObject)
            .Subscribe(_ => _isCharge = true);


        // 롱클릭 취소
        mouseLeftDownStream.Timestamp()
            .Zip(mouseLeftUpStream.Timestamp(), (d, u) => (u.Timestamp - d.Timestamp).TotalMilliseconds / 1000.0f)
            .Where(time => time < 3.0f)
            .Subscribe(t =>
            {
                //Debug.Log("롱클릭 취소");
                _curChargeTime = (float)t;
                Attack();
            });

        //  무기 활성화 및 비활성화 시키기

        mouseRightDownStream
            .Subscribe(_ => Ready());
        mouseRightUpStream
            .Subscribe(_ => ReadyCancel());*/
    }


    /// <summary>
    /// Attack으로 하나 묶어서 해버리는게 손해인가?
    /// </summary>
    private void Attack()
    {
        if (Character.Instance.IsInteract)
            return;

        if (!Animator.CanAttack || Animator.AnimState == CharacterAnimator.ChaAnimState.Roll)
        {
            return;
        }
        
        Animator.CanAttack = false;
        switch (Animator.AttackCount) // AttackCount
        {
            case 0:
                Attack_First();
                break;
            case 1:
                Attack_Second();
                break;
            default:
                break;
        }
    }

    private void Attack_First()
    {
        Animator.AnimState = CharacterAnimator.ChaAnimState.Attack;
        Animator.Anim_Sword_Slash1();
    }

    private void Attack_Second()
    {
        Animator.Anim_Sword_Slash2();
    }

    public void Attack_Charge()
    {
        //DebugManager.ins.Log("차지 공격 " + _curChargeTime + "초 차징함", DebugManager.TextColor.Yellow);
    }

    public void Attack_Ready()
    {
        //DebugManager.ins.Log("우클릭 후 공격", DebugManager.TextColor.Yellow);
    }

    public void Ready() // 우클릭 동작
    {
        //DebugManager.ins.Log("준비 자세", DebugManager.TextColor.Red);
        _isReady = true;
    }

    public void ReadyCancel()
    {
        //DebugManager.ins.Log("준비 취소", DebugManager.TextColor.Red);
        _isReady = false;
    }

    public void Run()
    {
        //DebugManager.ins.Log("Run", DebugManager.TextColor.Red);
        //Animator.WalkValue = 1.0f;
    }

    public void RunCancel()
    {
        //DebugManager.ins.Log("Run Cancel", DebugManager.TextColor.Red);
        //Animator.WalkValue = 0.0f;
    }

    public void Interact() // 상호작용
    {
        if (popup_Basic == null)
            popup_Basic = PopupManager.Instance.PopupList[0].GetComponent<UIPopupBasic>();

        popup_Basic._basicView.Input_Interact();

        /*
        if (Animator.AnimState == CharacterAnimator.ChaAnimState.Idle)
        {
        }*/
    }

    public void Pause() // BasicView -> Pause
    {
        if (popup_Basic == null)
            popup_Basic = PopupManager.Instance.PopupList[0].GetComponent<UIPopupBasic>();

        popup_Basic._basicView.Input_Pause();
    }

    public void GetPotion() // 물약 먹기
    {
        if (popup_Basic == null)
            popup_Basic = PopupManager.Instance.PopupList[0].GetComponent<UIPopupBasic>();

        if (Animator.AnimState == CharacterAnimator.ChaAnimState.Idle)
        {
            Animator.AnimState = CharacterAnimator.ChaAnimState.Drinking;
            Animator.Anim_Drinking();
            popup_Basic._basicView.GetPotion();
        }
    }

    public void Jump()
    {
        if (Character.Instance.IsInteract)
            return;
        if (Animator.AnimState == CharacterAnimator.ChaAnimState.Idle) //|| !Animator.IsJumping)
        {
            Animator.AnimState = CharacterAnimator.ChaAnimState.Jump;
            Animator.IsJumping = true;
            Animator.Anim_Jump();
            Movement.Jump();
            // 점프를 합니당~
        }
    }

    public void Roll()
    {
        if (Character.Instance.IsInteract)
            return;

        if (Animator.AnimState == CharacterAnimator.ChaAnimState.Idle) //) &&!Animator.IsRolling)
        {
            Animator.AnimState = CharacterAnimator.ChaAnimState.Roll;
            Animator.Anim_Roll();
            Movement.RollMove();
        }
    }

    public void PinTarget()
    {
        if (Character.Instance.IsInteract)
            return;
        if (Movement.IsPin) // Pin 활성화 되어있음
        {
            if (BattleManager.GetPinEnemyList().Count >= 1)
            {
                Movement.SetPinEnemy(null); // movement의 pinTarget 지우기
                Movement.IsPin = false;
            }
        }
        else // Pikkn 활성화가 안되어있을 때
        {
            if (BattleManager.GetPinEnemyList().Count >= 1)
            {
                Movement.SetPinEnemy(BattleManager.GetPinEnemyList()[0]);
                Movement.IsPin = true;
            }
        }
    }
}