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

    private bool _isCharge; // ��������(��Ŭ�� ��)
    private bool _isReady; // ��Ŭ���� �غ����

    private bool _isContinuous; // ���Ӱ����� �ϸ� �Ǵ���
    private void Start()
    {
        Animator = Character.Instance.Animator;
        Movement = Character.Instance.Movement;
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

        // ���� ȸ�� -> BasicView�� �̵�
        // InteractȰ��ȭ -> BasicView�� �̵�
        //var changeMdoeStrema = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.R)).Subscribe(_ => ChangeWeaponState());

        // ��Ŭ��
        mouseLeftDownStream
            // ���콺 Ŭ���Ǹ� 2���� OnNext ����
            .SelectMany(_ => Observable.Timer(TimeSpan.FromSeconds(_refChargeTime))) // _chargeTime��ŭ ��Ƽ� ������ ������
            .TakeUntil(mouseLeftUpStream) // ���߿� mouseUp �Ǹ� ��Ʈ�� �ʱ�ȭ
            .RepeatUntilDestroy(gameObject)
            .Subscribe(_ => _isCharge = true);


        // ��Ŭ�� ���
        mouseLeftDownStream.Timestamp()
            .Zip(mouseLeftUpStream.Timestamp(), (d, u) => (u.Timestamp - d.Timestamp).TotalMilliseconds / 1000.0f)
            .Where(time => time < 3.0f)
            .Subscribe(t =>
            {
                //Debug.Log("��Ŭ�� ���");
                _curChargeTime = (float)t;
                Attack();
            });

        //  ���� Ȱ��ȭ �� ��Ȱ��ȭ ��Ű��

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
        Attack_Normal();
        /*
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
        }*/
    }

    public void Attack_Normal()
    {
        if (Character.Instance.IsInteract)
            return;

        if (!Animator.CanAttack || Animator.AnimState == CharacterAnimator.ChaAnimState.Roll)
        {
            //Debug.Log("���� ��ȯ����");
            return;
        }
        Animator.CanAttack = false;
        //if (Animator.AnimState != CharacterAnimator.ChaAnimState.SeriesAttackReady) // ���Ӱ��� ���� �ƴ� ( 1Ÿ ) 
        if(Animator.AttackCount == 0)
        {
            //Debug.Log("����_1Ÿ");
            Animator.AnimState = CharacterAnimator.ChaAnimState.Attack;
            Animator.Anim_Sword_Slash1();
            
            //Animator.IsAttacking = true;
            //_isContinuous = true;
        }
        else if(Animator.AttackCount == 1) // ���Ӱ��� ����
        {
            //Animator.IsAttacking = true;
            //Debug.Log("����_2Ÿ");
            Animator.Anim_Sword_Slash2();
        }


    }
    public void Attack_Charge()
    {
        //DebugManager.ins.Log("���� ���� " + _curChargeTime + "�� ��¡��", DebugManager.TextColor.Yellow);
    }
    public void Attack_Ready()
    {
        //DebugManager.ins.Log("��Ŭ�� �� ����", DebugManager.TextColor.Yellow);
    }
    public void Ready() // ��Ŭ�� ����
    {
        //DebugManager.ins.Log("�غ� �ڼ�", DebugManager.TextColor.Red);
        _isReady = true;
    }
    public void ReadyCancel()
    {
        //DebugManager.ins.Log("�غ� ���", DebugManager.TextColor.Red);
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
            // ������ �մϴ�~
        }
    }
    public void Roll()
    {
        if (Character.Instance.IsInteract)
            return;
        if (Animator.AnimState == CharacterAnimator.ChaAnimState.Idle)//) &&!Animator.IsRolling)
        {
            Animator.AnimState = CharacterAnimator.ChaAnimState.Roll;
            Animator.IsRolling = true;
            //DebugManager.ins.Log("������ �ִϸ��̼�", DebugManager.TextColor.Blue);
            Animator.Anim_Roll();
            Movement.RollMove();
        }
        //else
        //DebugManager.ins.Log("�ִϸ��̼� ������", DebugManager.TextColor.White);
    }
    public void PinTarget()
    {
        if (Character.Instance.IsInteract)
            return;
        if (Movement.IsPin) // Pin Ȱ��ȭ �Ǿ�����
        {
            if (BattleManager.GetPinEnemyList().Count >= 1)
            {
                Movement.SetPinEnemy(null); // movement�� pinTarget �����
                Movement.IsPin = false;
            }
        }
        else // Pikkn Ȱ��ȭ�� �ȵǾ����� ��
        {
            if (BattleManager.GetPinEnemyList().Count >= 1)
            {
                Movement.SetPinEnemy(BattleManager.GetPinEnemyList()[0]);
                Movement.IsPin = true;
            }
        }
    }
}
