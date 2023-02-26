using System;
using UniRx.Triggers;
using UnityEngine;
using UniRx;

public class InputManager : MonoBehaviour
{
    private CharacterMovement characterMovement;
    // Input
    private float _refChargeTime = 0.7f;
    private float _curChargeTime;

    private bool _isCharge; // ��������(��Ŭ�� ��)
    private bool _isReady; // ��Ŭ���� �غ����
    private void Start()
    {
        characterMovement = this.GetComponent<CharacterMovement>();
        InputSetting();
    }
    private void InputSetting()
    {
        var mouseLeftDownStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));
        var mouseLeftUpStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonUp(0));

        var mouseRightDownStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(1));
        var mouseRightUpStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonUp(1));

        var targetPinStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.Q));

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

        targetPinStream.Subscribe(_ => PinTarget());
    }


    /// <summary>
    /// Attack���� �ϳ� ��� �ع����°� �����ΰ�?
    /// </summary>
    private void Attack()
    {

        if (_isReady)
        {
            Attack_Ready();
            return;
        }

        if (_isCharge)
        {
            Attack_Charge();
            return;
        }
        else
        {
            Attack_Normal();
            return;
        }
    }
    public virtual void Attack_Normal()
    {
        DebugManager.ins.Log("�Ϲ� ����", DebugManager.TextColor.Yellow);
    }
    public virtual void Attack_Charge()
    {
        DebugManager.ins.Log("���� ���� " + _curChargeTime + "�� ��¡��", DebugManager.TextColor.Yellow);
    }
    public void Attack_Ready()
    {
        DebugManager.ins.Log("��Ŭ�� �� ����", DebugManager.TextColor.Yellow);

    }

    public virtual void Ready() // ��Ŭ�� ����
    {
        DebugManager.ins.Log("�غ� �ڼ�", DebugManager.TextColor.Red);
        _isReady = true;
    }
    public void ReadyCancel()
    {
        DebugManager.ins.Log("�غ� ���", DebugManager.TextColor.Red);
        _isReady = false;
    }
    public virtual void PinTarget()
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
