using Assets.Scripts.Manager;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private WeaponController weaponController; 

    private Animator _animator;
   
    // �����⵿���� input�� ������ �ȵǰ�, �������϶��� �̵��ӵ� ���Ҹ� �ؾߵǴϱ� �ٸ� ������ �Ҵ���
    private bool _isRolling;
    private bool _isJumping;
    private bool _isAttacking; // ������(������ ����)
    private bool _isStateChnaging; // �ִϸ����� ���̾� ������

    private int _attackCount; // ���Ӱ��ݿ� ���
    public int AttackCount
    { get { return _attackCount; } }
    
    private bool _canAttack =true;
    public bool CanAttack
    {
        get { return _canAttack; }
        set 
        {
            if (_canAttack != value)
                _canAttack = value;
        }
    }
    public Animator animator
    {
        get { return _animator; }
    }

    // �̵� �� �ٸ� ��� ������ �� ->  MultiMoveAnim (�ȱ�� ��ġ�� �ȵǱ���)
    // �Ұ� -> OnlyThisAnim
    public enum ChaAnimState
    {
        Idle, // ���� ���� ����
        Jump, // ����,
        Roll, //  ������
        Attack, // ������
        Drinking, // ȸ�������
        //SeriesAttackReady, // ���Ӱ��� �غ�
    }
    

    public ChaAnimState AnimState = ChaAnimState.Idle;

    #region ::: �ִϽ�����Ʈ ���ϰ� �������� ���ؼ� �̸� bool �Լ��� �����صα�

    /// <summary> ������ �ƹ��͵� ���Ҷ��� �̵��ǵ����� </summary>
    /// <returns>Idle, Jump </returns>
    public bool CanMove()
    {
        return AnimState == ChaAnimState.Idle || AnimState == ChaAnimState.Jump;
    }
    public bool CanAttak()
    {
        return AnimState == ChaAnimState.Idle;
    }
    public bool CanHeal()
    {
        return AnimState == ChaAnimState.Idle;
    }

    #endregion
    public bool IsRolling
    {
        get { return _isRolling; }
        set { if (_isRolling != value) _isRolling = value; }
    }
    public bool IsJumping
    {
        get { return _isJumping; }
        set
        { if (_isJumping != value) _isJumping = value; }
    }
    public bool IsAttacking
    {
        get { return _isAttacking; }
        set
        { if (_isAttacking != value) _isAttacking = value; }
    }
    public bool IsStateChnaging
    {
        get { return _isStateChnaging; }
        set
        { if (_isStateChnaging != value) _isStateChnaging = value; }
    }
    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
        //weaponController = Character.Instance.weaponController;
    }

    #region ::: Walk :::
    public void Blend_Walk(float value)
    {
        _animator.SetFloat("WalkBlend", value);
    }
    public void Anim_GetDirection(float dirx, float diry)
    {
        animator.SetFloat("DirX", dirx);
        animator.SetFloat("DirY", diry);
    }
    #endregion

    #region ::: Roll :::
    public void Anim_Roll()
    {
        _animator.SetTrigger("Roll");
        IsRolling = true;
    }
    public void End_Anim_Roll()
    {
        IsRolling = false;
        AnimState = ChaAnimState.Idle;
    }
    #endregion


    public void Anim_Jump()
    {
        _animator.SetTrigger("Jump");
    }
    public void Anim_Idle()
    {
        //_animator.SetTrigger("Idle");
        _animator.SetFloat("WalkBlend", 0f);
    }



    #region ##### Sword #####
    public void Anim_Sword_Slash1()
    {
        _animator.SetTrigger("Slash1");
        weaponController.SwordAnim_Show();
        _attackCount++;
    }
    public void Anim_Sword_Slash2()
    {
        _animator.SetTrigger("Slash2");
    }
    public void Anim_DamagePoint() // �������� �ִ� ����
    {
        // Active Enemy is Enable
        if (BattleManager.GetEnemy().Count > 0)
        {
            BattleManager.Attack();
        }
        Anim_SeriesAttack_Active();
    }
    private void Anim_SeriesAttack_Active() // ���Ӱ��� Ȱ��ȭ
    {
        if(_attackCount == 0) // ���� ���� 
        {
            CanAttack = true; // �ٽ� ���� �����ϰ� �������
        }
        else // �ι�° Ÿ�ݿ����� ������ �ٽ� Ȱ��ȭ�Ǹ� �ȵǴϱ�
        { return; }
        
    }
    #endregion


    public void Anim_Sword_Death1()
    {
        _animator.SetTrigger("Death1");
    }
    public void Anim_Sword_Death2()
    {
        _animator.SetTrigger("Death2");
    }

    #region ::: Sitting :::
    public void Anim_Sitting()
    {
        _animator.SetTrigger("Sitting");
    }
    public void SittingEnd()
    {

    }

    public void Anim_Drinking()
    {
        _animator.SetTrigger("Drinking");
    }
    public void DrinkingEnd()
    {
        // �� ������ �Լ��� ���� �͵��� ȸ�� �߿� �ǰݵǾ��� ��쿡�� ȣ��Ǿ����
        AnimState = ChaAnimState.Idle;
    }

    #endregion


    public void End_Anim_Jump() // ������ �ִϸ��̼� ���Ḧ �ǹ�
    {
        AnimState = ChaAnimState.Idle;
    }
    public void Jump_GravityActive() // �߷� ���� ���� 
    {
        IsJumping = false;
    }

    public void End_Anim_Slash()
    {
        _attackCount = 0;
        AnimState = ChaAnimState.Idle;
        CanAttack = true;
        weaponController.TimerStart();
    }
}
