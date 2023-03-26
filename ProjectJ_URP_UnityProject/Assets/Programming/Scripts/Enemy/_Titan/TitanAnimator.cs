
public class TitanAnimator : EnemyAnimator
{
    public override void Anim_Attack()
    {
        base.Anim_Attack();
    }
    public void Anim_Attack2()
    {
        _animator.SetTrigger("Attack2");
    }
    public void Anim_Attack3()
    {
        _animator.SetTrigger("Attack3");
    }
    public override void Anim_DamagePoint()
    {
        base.Anim_DamagePoint();
    }

    // Attack3���� ����� �̵��ϸ鼭 ������
    public void Anim_MovePoint()
    {
        // ȣ��ɶ����� �������� ��ŭ �÷��������� �̵�, �Ÿ� ���� �ּ� �����Ÿ� �̻��϶��� ��������
    }

    public override void Anim_Death()
    {
        base.Anim_Death();
    }

    #region ::: JumpAttack :::
    public void JumpStartPoint()
    {
        // ����� �� ���� ������ ��ġ�� ���� �Ҵ�
        _movement.AI_Doing_Jump();
    }
    public void JumpAfterMovePoint()
    {
        _movement.AI_Doing_JumpAfterMove();
    }
    #endregion
    public override void Anim_Jump()
    {
        base.Anim_Jump();
    }
    public override void Anim_Damaged()
    {
        base.Anim_Damaged();
    }
    public override void End_Anim()
    {
        base.End_Anim();
    }
}
