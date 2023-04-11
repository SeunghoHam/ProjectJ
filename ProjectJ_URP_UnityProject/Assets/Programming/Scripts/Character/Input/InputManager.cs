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

    private bool _isCharge; // ��������(��Ŭ�� ��)
    private bool _isReady; // ��Ŭ���� �غ����

    private bool _isContinuous; // ���Ӱ����� �ϸ� �Ǵ���

    private UIPopupBasic popup_Basic; // BasicView���� ���۽�ų�͵� ������

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

        // ���� ȸ�� -> BasicView�� �̵�
        // InteractȰ��ȭ -> BasicView�� �̵�
        var potionStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.R))
            .Subscribe(_ => GetPotion());
        var interactStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.E))
            .Subscribe(_ => Interact());
        var pauseStream = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.P)).Subscribe(_ => Pause());

        /* [About] LongClick
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
            .Subscribe(_ => ReadyCancel());*/
    }


    /// <summary>
    /// Attack���� �ϳ� ��� �ع����°� �����ΰ�?
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

    public void Interact() // ��ȣ�ۿ�
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

    public void GetPotion() // ���� �Ա�
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
            // ������ �մϴ�~
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