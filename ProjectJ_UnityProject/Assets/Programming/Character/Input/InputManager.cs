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

    private bool _isCharge; // ��������(��Ŭ�� ��)
    private bool _isReady; // ��Ŭ���� �غ����
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

        // ��Ŭ��
        mouseLeftDownStream
            // ���콺 Ŭ���Ǹ� 2���� OnNext ����
            .SelectMany(_ => Observable.Timer(TimeSpan.FromSeconds(_refChargeTime))) // _chargeTime��ŭ ��Ƽ� ������ ������
            .TakeUntil(mouseLeftUpStream) // ���߿� mouseUp �Ǹ� ��Ʈ�� �缳��
            .RepeatUntilDestroy(gameObject)
           .Subscribe(_ => _isCharge = true);

        // ��Ŭ�� ���
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
    /// Attack���� �ϳ� ��� �ع����°� �����ΰ�?
    /// </summary>
    private void Attack()
    {
        if (_isReady) // ��Ŭ�� �� ����
        {
            Attack_Ready();
            return;
        }

        if (_isCharge) // ���� ����
        {
            Attack_Charge();
            return;
        }
        else
        {
            Attack_Normal(); // �׳� ���� - ���Ӱ��� ����� ����
            return;
        }
    }
    public void Attack_Normal()
    {
        DebugManager.ins.Log("�Ϲ� ����", DebugManager.TextColor.Yellow);
    }
    public void Attack_Charge()
    {
        DebugManager.ins.Log("���� ���� " + _curChargeTime + "�� ��¡��", DebugManager.TextColor.Yellow);
    }
    public void Attack_Ready()
    {
        DebugManager.ins.Log("��Ŭ�� �� ����", DebugManager.TextColor.Yellow);

    }


    public void Ready() // ��Ŭ�� ����
    {
        DebugManager.ins.Log("�غ� �ڼ�", DebugManager.TextColor.Red);
        _isReady = true;
    }
    public void ReadyCancel()
    {
        DebugManager.ins.Log("�غ� ���", DebugManager.TextColor.Red);
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
            DebugManager.ins.Log("Ÿ�� ����", DebugManager.TextColor.White);
        }
        else
        {
            DebugManager.ins.Log("����Ʈ �� ���� ���� ����", DebugManager.TextColor.White);
        }
    }


}
