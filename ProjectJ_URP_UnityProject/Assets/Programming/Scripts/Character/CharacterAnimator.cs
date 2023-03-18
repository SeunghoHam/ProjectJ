using Assets.Scripts.Manager;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private WeaponController weaponController; 

    private Animator _animator;
    private float _walkBlend;
   
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
        //SeriesAttackReady, // ���Ӱ��� �غ�
    }
    

    public ChaAnimState AnimState = ChaAnimState.Idle;

    #region ::: �ִϽ�����Ʈ ���ϰ� �������� ���ؼ� �̸� bool �Լ��� �����صα�

    /// <summary> ������ �ƹ��͵� ���Ҷ��� �̵��ǵ����� </summary>
    /// <returns>Idle, Jump </returns>
    public bool ReturnCanMove()
    {
        return AnimState == ChaAnimState.Idle || AnimState == ChaAnimState.Jump;
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

    public void Blend_Walk(float value)
    {
        _animator.SetFloat("WalkBlend", value);
    }
    public void Anim_GetDirection(float dirx, float diry)
    {
        animator.SetFloat("DirX", dirx);
        animator.SetFloat("DirY", diry);
    }

    public void Anim_Roll()
    {
        _animator.SetTrigger("Roll");
    }

    public void Anim_Jump()
    {
        _animator.SetTrigger("Jump");
    }
    public void Anim_Idle()
    {
        _animator.SetTrigger("Idle");
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
        
        if (BattleManager.GetEnemy().Count > 0)
        {
            BattleManager.Attack();
            //Character.Instance.Attack();
        }
        Anim_SeriesAttack_Active();
    }
    public void Anim_SeriesAttack_Active() // ���Ӱ��� Ȱ��ȭ
    {
        if(AnimState != ChaAnimState.Attack)
        {
            Debug.Log("�ٽ� ���ݰ���");
            CanAttack = true; // �ٽ� ���� �����ϰ� �������
        }
        else // �ι�° Ÿ�ݿ����� ������ �ٽ� Ȱ��ȭ�Ǹ� �ȵǴϱ�
        { return; }
        
    }
    public void Anim_SeriesAttack_DeActive() // ���Ӱ��� ��Ȱ��ȭ ( EndSlash ���� �̿����� �ص� ���� ������?)
    {

    }

    public void Anim_Sword_Death1()
    {
        _animator.SetTrigger("Death1");
    }
    public void Anim_Sword_Death2()
    {
        _animator.SetTrigger("Death2");
    }
    public void Anim_Sword_()
    {
        
    }
    #endregion


    public void End_Anim_Roll() // �ִϸ����� Ŭ���� �Ҵ��Ұ�
    {
        IsRolling = false;
        AnimState = ChaAnimState.Idle;
    }
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
        //IsAttacking = false;
        _attackCount = 0;
        AnimState = ChaAnimState.Idle;
        CanAttack = true;
        weaponController.TimerStart();
    }
}
