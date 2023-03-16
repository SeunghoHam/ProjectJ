using System;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
using System.Collections;
using Assets.Scripts.Manager;

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
    private void Start()
    {
        Animator = Character.Instance.characterAnimator;
        Movement = Character.Instance.characterMovement;
        weaponController = Character.Instance.weaponController;

        InputSetting();
    }
    private void InputSetting()
    {
        var mouseLeftDownStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));
        var mouseLeftUpStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonUp(0));

        var mouseRightDownStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(1));
        var mouseRightUpStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonUp(1));

        var targetPinStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.Q)).Subscribe(_ => PinTarget());
        var jumpStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.F)).Subscribe(_ => Jump());
        var rollStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.Space)).Subscribe(_ => Roll());
        var runStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.LeftShift)).Subscribe(_ => Run());
        var runCancelStrema = this.UpdateAsObservable().Where(_ => Input.GetKeyUp(KeyCode.LeftShift)).Subscribe(_ => RunCancel());

        var itemGetStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.E)).Subscribe(_ => GetPosition());
        //var changeMdoeStrema = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.R)).Subscribe(_ => ChangeWeaponState());

        // 롱클릭
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
                Debug.Log("롱클릭 취소");
                _curChargeTime = (float)t;
                Attack();
            });

        //  무기 활성화 및 비활성화 시키기

        mouseRightDownStream
            .Subscribe(_ => Ready());
        mouseRightUpStream
            .Subscribe(_ => ReadyCancel());
    }



    private void ChangeWeaponState()
    {
        if (Character.Instance.weaponState == Character.WeaponState.Normal)
        {
            Character.Instance.weaponState = Character.WeaponState.Sword;
            Animator.IsStateChnaging = true;
            //Animator.animator.layer
        }
        else
        {
            Character.Instance.weaponState = Character.WeaponState.Normal;
            Animator.IsStateChnaging = true;
        }
        DebugManager.ins.Log("현재 weaponMode 는 " + Character.Instance.weaponState.ToString(), DebugManager.TextColor.Blue);
    }
    /// <summary>
    /// Attack으로 하나 묶어서 해버리는게 손해인가?
    /// </summary>
    private void Attack()
    {
        Attack_Normal();
        /*
        if (_isReady) // 우클릭 후 공격
        {
            Attack_Ready();
            return;
        }

        if (_isCharge) // 차지 공격
        {
            Attack_Charge();
            return;
        }
        else
        {
            Attack_Normal(); // 그냥 공격 - 연속공격 생기면 적용
            return;
        }*/
    }

    public void Attack_Normal()
    {
        if (!Animator.CanAttack || Animator.AnimState == CharacterAnimator.ChaAnimState.Roll)
        {
            Debug.Log("공격 반환당함");
            return;
        }
        Animator.CanAttack = false;
        if (Animator.AnimState != CharacterAnimator.ChaAnimState.SeriesAttackReady) // 연속공격 중이 아님 ( 1타 ) 
        {
            Debug.Log("공격_1타");
            Animator.AnimState = CharacterAnimator.ChaAnimState.SeriesAttackReady;
            Animator.Anim_Sword_Slash1();
            //Animator.IsAttacking = true;
            //_isContinuous = true;
        }
        else // 연속공격 적용
        {
            Animator.AnimState = CharacterAnimator.ChaAnimState.Attack;
            //Animator.IsAttacking = true;
            Debug.Log("공격_2타");
            Animator.Anim_Sword_Slash2();
        }


    }
    public void Attack_Charge()
    {
        DebugManager.ins.Log("차지 공격 " + _curChargeTime + "초 차징함", DebugManager.TextColor.Yellow);
    }
    public void Attack_Ready()
    {
        DebugManager.ins.Log("우클릭 후 공격", DebugManager.TextColor.Yellow);
    }
    public void Ready() // 우클릭 동작
    {
        DebugManager.ins.Log("준비 자세", DebugManager.TextColor.Red);
        _isReady = true;
    }
    public void ReadyCancel()
    {
        DebugManager.ins.Log("준비 취소", DebugManager.TextColor.Red);
        _isReady = false;
    }

    public void Run()
    {
        //DebugManager.ins.Log("Run", DebugManager.TextColor.Red);
        Animator.WalkValue = 1.0f;
    }
    public void RunCancel()
    {
        //DebugManager.ins.Log("Run Cancel", DebugManager.TextColor.Red);
        Animator.WalkValue = 0.0f;
    }
    public void Jump()
    {
        if (Animator.ReturnCanJump())//|| !Animator.IsJumping)
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
        if (Animator.AnimState != CharacterAnimator.ChaAnimState.Roll)//) &&!Animator.IsRolling)
        {
            Animator.AnimState = CharacterAnimator.ChaAnimState.Roll;
            Animator.IsRolling = true;
            //DebugManager.ins.Log("구르기 애니메이션", DebugManager.TextColor.Blue);
            Animator.Anim_Roll();
            Movement.RollMove();
        }
        //else
        //DebugManager.ins.Log("애니메이션 동작중", DebugManager.TextColor.White);
    }
    public void GetPosition()
    {
        DebugManager.ins.Log("물약 먹기", DebugManager.TextColor.White);
    }

    public void PinTarget()
    {
        /*
        if (Character._enemyList.Count >= 1)
        {
            if (Movement.IsPin) // Pin 활성화 되어있음
            {
                DebugManager.ins.Log("타겟 활성화 해제하기", DebugManager.TextColor.White);
                Movement.SetPinEnemy(null); // movement의 pinTarget 지우기
                Movement.IsPin = false;
            }
            else // Pikkn 활성화가 안되어있을 때
            {
                DebugManager.ins.Log("타겟 활성화 시키기", DebugManager.TextColor.White);
                Movement.SetPinEnemy(Character.GetEnemy());
                Movement.IsPin = true;
            }
        }
        else
        {
            DebugManager.ins.Log("리스트 비어서 동작 안함 ㅅㄱ", DebugManager.TextColor.White);
            Movement.IsPin = false;
        }*/
    }
}
