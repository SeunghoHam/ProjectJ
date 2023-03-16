using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.Manager;
using UniRx.Triggers;

public class EnemyMovement : MonoBehaviour
{
    // ������ �� ȸ�� ����

    private Transform model; // ȸ���� ������Ʈ
    [SerializeField] private Transform target; // ���� ��� ( ��ũ��Ʈ���� �޾ƿ� �� �ְ� �ؾ���)
    private EnemyAnimator animator;

    private Rigidbody rigid;
    private bool isDoing = false; // ������
    private float _jumpForce = 3f;
    private float _moveDuration = 0.5f; // �̵��� �ɸ��� �ð�
    private void Awake()
    {
        model = this.transform.GetChild(0).transform;
        rigid = this.GetComponent<Rigidbody>();
        //target = Character.Instance.gameObject.transform;

        
    }
    public void Initialize(EnemyAnimator Anim) // Enemy ��ũ��Ʈ���� �ʿ��� ������ ��������
    {
        animator = Anim;
    }



    private void FixedUpdate()
    {
        if (target == null)
            return;

        if(!isDoing)
        {
            Vector3 targetAxis = new Vector3(target.transform.position.x, this.transform.position.y, transform.position.z);
            model.LookAt(target, Vector3.up);
        }
       
        if(Input.GetKeyDown(KeyCode.R))
        {
            AI_Doing_JumpAttack();
        }
    }

    public void AI_Doing_Avoid() // �÷��̾ �׳� �����ϸ� ���ع���
    {
        // �÷��̾��� ��ġ - ��(��)�� ��ġ = �̵��ؾ��� ������ ���͸� ��ȯ

        // ���ⱸ�ϱ�
        // ���� ���Ͱ� = ��ǥ ���Ͱ� - ���ۺ���
        Vector3 heading = (this.transform.position - target.position).normalized;
        Vector3 dir = this.transform.position +  heading * 1.2f;
        this.transform.DOMove(dir, 0.5f, false);
    }
    public void AI_Doing_JumpAttack()
    {
        animator.Anim_Jump();
        rigid.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        //rigid.MovePosition(target.position);
        MoveAttack();
    }
    public void  AI_Doing_Jump()
    {
        animator.Anim_Jump();
        rigid.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
    private void MoveAttack()
    {
        this.transform.DOMove(target.position, _moveDuration, false);
        animator.Anim_Slash();
    }
}
