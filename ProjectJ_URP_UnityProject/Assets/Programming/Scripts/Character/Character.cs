using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : UnitBase
{
    public PinTargetRange pinTargetRange;
    public CharacterMovement Movement;
    public CharacterAnimator Animator;
    public CameraSystem cameraSystem;
    public WeaponController weaponController;

    public static Character Instance;
    
    private int _maxHP = 10;
    private int _curHP;

    
    #region  ::: CharacterData :::

    public string userName;
    public float exp;
    public int[] status;

    #endregion
    
    // Can Interact?
    #region ::: Interact :::

    private bool _canInteract = false;
    public bool CanInteract
    {
        get { return _canInteract; }
        set 
        {
            if(_canInteract != value)
                _canInteract = value;
        }
    }
    private bool _isInteract = false;
    public bool IsInteract
    {
        get { return _isInteract; }
        set
        {
            if(_isInteract != value)
            {
                Animator.Anim_Idle();
                _isInteract = value;
            }
        }
    }

    #endregion

    // 값이 변경될 때 마다 BasicView에서 hpStatus 변경함
    public int MaxHP
    {
        get { return _maxHP; }
    }

    public int CurHP
    {
        get { return _curHP; }
        set
        {
            _curHP = value;
        }
    }

    // CharacterState : Normal / Sword
    public enum WeaponState
    { 
        Normal,
        Sword,
    }
    public WeaponState weaponState = WeaponState.Normal;

    [Space(10)]
    public Transform TargetPoint;

    private int _attackDamage = 1;
    public int AttackDamage
    { get{ return _attackDamage; } }
    

    public CameraSystem GetCameraSystem
    {
        get { return cameraSystem; }
        set 
        { if (cameraSystem == null)
                cameraSystem = value;
        }
    }


    private void Awake()
    {
        Instance = this;
        _curHP = _maxHP;
    }
    public override void Attack()
    {
        base.Attack();
    }
    public override void Damaged(int damage)
    {
        base.Damaged(damage);
        CurHP -= damage;

        if(_curHP <= 0 )
        {
            Debug.Log("플레이어 주거써");
        }
    }
    public override void Avoid()
    {
        base.Avoid();
    }
}
