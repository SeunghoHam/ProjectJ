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


    public static Character Instance;
    

    // CharacterState : Normal / Sword
    public enum WeaponState
    { 
        Normal,
        Sword,
    }
    public WeaponState weaponState = WeaponState.Normal;

    [Space(10)]
    public Transform TargetPoint;

    private void Awake()
    {
        Instance = this;
    }
    public CameraSystem GetCameraSystem
    {
        get { return cameraSystem; }
        set 
        { if (cameraSystem == null)
                cameraSystem = value;
        }
    }
}
