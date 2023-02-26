using System;
using UniRx.Triggers;
using UnityEngine;
using UniRx;

public class InputManager : MonoBehaviour
{
    private CharacterMovement Movement;
    private CharacterAnimator Animator;
    // Input
    private float _refChargeTime = 0.7f;
    private float _curChargeTime;

    private bool _isCharge; // 차지공격(좌클릭 꾹)
    private bool _isReady; // 우클릭시 준비상태
    private void Start()
    {
        Movement = this.GetComponent<CharacterMovement>();
        Animator = Character.Instance.characterAnimator;
        InputSetting();
    }
    private void InputSetting()
    {
        var mouseLeftDownStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));
        var mouseLeftUpStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonUp(0));

        var mouseRightDownStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(1));
        var mouseRightUpStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonUp(1));

        var targetPinStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.Q)).Subscribe(_ => PinTarget());
        var jumpStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.Space)).Subscribe(_=> Jump());
        var rollStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.LeftControl)).Subscribe(_=> Roll());
        var runStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.LeftShift)).Subscribe(_=> Run());
        var runCancelStrema = this.UpdateAsObservable().Where(_ => Input.GetKeyUp(KeyCode.LeftShift)).Subscribe(_ => RunCancel());

        // 롱클릭
        mouseLeftDownStream
            // 마우스 클릭되면 2초후 OnNext 실행
            .SelectMany(_ => Observable.Timer(TimeSpan.FromSeconds(_refChargeTime))) // _chargeTime만큼 모아서 때리면 강공격
            .TakeUntil(mouseLeftUpStream) // 도중에 mouseUp 되면 스트림 재설정
            .RepeatUntilDestroy(gameObject)
           .Subscribe(_ => _isCharge = true);

        // 롱클릭 취소
        mouseLeftDownStream.Timestamp()
            .Zip(mouseLeftUpStream.Timestamp(), (d, u) => (u.Timestamp - d.Timestamp).TotalMilliseconds / 1000.0f)
            .Where(time => time < 3.0f)
            .Subscribe(t =>
            {
                _curChargeTime = (float)t;
                Attack();
            });
        mouseRightDownStream
            .Subscribe(_ => Ready());
        mouseRightUpStream
            .Subscribe(_ => ReadyCancel());
    }


    /// <summary>
    /// Attack으로 하나 묶어서 해버리는게 손해인가?
    /// </summary>
    private void Attack()
    {
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
        }
    }
    public void Attack_Normal()
    {
        DebugManager.ins.Log("일반 공격", DebugManager.TextColor.Yellow);
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
        //characterAnimator.Anim_Jump();
        Animator.Anim_Jump();
    }
    public void Roll()
    {
        //characterAnimator.Anim_Roll();
        Animator.Anim_Roll();
    }

    public void PinTarget()
    {
        if(Character._enemyList.Count >=1 )
        {
            DebugManager.ins.Log("타겟 동작", DebugManager.TextColor.White);
        }
        else
        {
            DebugManager.ins.Log("리스트 비어서 동작 안함 ㅅㄱ", DebugManager.TextColor.White);
        }
    }


}
