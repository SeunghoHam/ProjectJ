using System;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
using Unity.VisualScripting;
using System.Collections;
using UnityEditor.Experimental.GraphView;

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
        Animator = Character.Instance.characterAnimator;
        Movement = Character.Instance.characterMovement;
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
        var changeMdoeStrema = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.R)).Subscribe(_ => ChangeWeaponState());

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



    private void ChangeWeaponState()
    {
        if(Character.Instance.weaponState == Character.WeaponState.Normal)
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
        DebugManager.ins.Log("���� weaponMode �� " + Character.Instance.weaponState.ToString(), DebugManager.TextColor.Blue);
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
        if (Animator.AnimState == CharacterAnimator.ChaAnimState.Idle)//|| !Animator.IsJumping)
        {
            Animator.AnimState = CharacterAnimator.ChaAnimState.Doing;
            Animator.IsAttacking = true;
            DebugManager.ins.Log("�Ϲ� ����", DebugManager.TextColor.Yellow);
            Animator.Anim_Sword_Slash();
        }
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
        if(Animator.AnimState == CharacterAnimator.ChaAnimState.Idle)//|| !Animator.IsJumping)
        {
            Animator.AnimState = CharacterAnimator.ChaAnimState.Doing;
            Animator.IsJumping = true;
            Animator.Anim_Jump();
            Movement.Jump();
            // ������ �մϴ�~
        }
        
    }
    public void Roll()
    {
        if(Animator.AnimState == CharacterAnimator.ChaAnimState.Idle)//) &&!Animator.IsRolling)
        {
            Animator.AnimState = CharacterAnimator.ChaAnimState.Doing;
            Animator.IsRolling = true;
            //DebugManager.ins.Log("������ �ִϸ��̼�", DebugManager.TextColor.Blue);
            Animator.Anim_Roll();
            Movement.RollMove();
        }
        //else
            //DebugManager.ins.Log("�ִϸ��̼� ������", DebugManager.TextColor.White);
    }
    public void GetPosition()
    {
        DebugManager.ins.Log("���� �Ա�", DebugManager.TextColor.White);
    }
    public void PinTarget()
    {
        if (Character._enemyList.Count >= 1)
        {
            if (Movement.IsPin) // Pin Ȱ��ȭ �Ǿ�����
            {
                DebugManager.ins.Log("Ÿ�� Ȱ��ȭ �����ϱ�", DebugManager.TextColor.White);
                Movement.SetPinEnemy(null); // movement�� pinTarget �����
                Movement.IsPin = false;
            }
            else // Pikkn Ȱ��ȭ�� �ȵǾ����� ��
            {
                DebugManager.ins.Log("Ÿ�� Ȱ��ȭ ��Ű��", DebugManager.TextColor.White);
                Movement.SetPinEnemy(Character.GetEnemy());
                Movement.IsPin = true;
            }

        }
        else
        {
            DebugManager.ins.Log("����Ʈ �� ���� ���� ����", DebugManager.TextColor.White);
            Movement.IsPin = false;
        }
    }


}
