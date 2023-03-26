
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

    // Attack3에서 사용할 이동하면서 때리기
    public void Anim_MovePoint()
    {
        // 호출될때마다 일정벡터 만큼 플레이이한테 이동, 거리 제한 둬서 일정거리 이상일때는 못가도록
    }

    public override void Anim_Death()
    {
        base.Anim_Death();
    }

    #region ::: JumpAttack :::
    public void JumpStartPoint()
    {
        // 모션이 길어서 점프 시작할 위치를 따로 할당
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
