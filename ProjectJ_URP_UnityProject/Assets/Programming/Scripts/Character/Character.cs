using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : UnitBase
{
    // PinTarget
    public PinTargetRange pinTargetRange;
    public CharacterMovement characterMovement;
    public CharacterAnimator characterAnimator;
    public CameraSystem cameraSystem;
    public WeaponController weaponController;

    public static Character Instance;
    
    private int _maxHP = 10;
    private int _curHP;

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
    private void Awake()
    {
        Instance = this;
        _curHP = _maxHP;
    }
    public CameraSystem GetCameraSystem
    {
        get { return cameraSystem; }
        set 
        { if (cameraSystem == null)
                cameraSystem = value;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            Damaged(1);
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
