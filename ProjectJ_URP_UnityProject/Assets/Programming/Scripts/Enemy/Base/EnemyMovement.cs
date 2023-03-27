using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.Manager;
using UniRx.Triggers;
using UniRx;
using Unity.VisualScripting;

public class EnemyMovement : MonoBehaviour
{
    // ������ �� ȸ�� ����
    private Transform model; // ȸ���� ������Ʈ
    [SerializeField] private GameObject target; // ���� ��� ( ��ũ��Ʈ���� �޾ƿ� �� �ְ� �ؾ���)
    private EnemyAnimator animator;
    private Rigidbody rigid;
    private bool isDoing = false; // ������
    private float _jumpForce = 6f;
    private float _moveDuration = 0.5f; // �̵��� �ɸ��� �ð�
    private void Awake()
    {
        model = this.transform.GetChild(0).transform;
        rigid = this.GetComponent<Rigidbody>();
        //target = Character.Instance.gameObject;
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
            model.LookAt(target.transform, Vector3.up);
            //model.transform.localRotation = Quaternion.Euler(0, model.transform.localRotation.y,0);
        }
    }

    public void AI_Doing_Avoid() // �÷��̾ �׳� �����ϸ� ���ع���
    {
        // �÷��̾��� ��ġ - ��(��)�� ��ġ = �̵��ؾ��� ������ ���͸� ��ȯ

        // ���ⱸ�ϱ�
        // ���� ���Ͱ� = ��ǥ ���Ͱ� - ���ۺ���
        Vector3 heading = (this.transform.position - target.transform.position).normalized;
        Vector3 dir = this.transform.position +  heading * 1.2f;
        this.transform.DOMove(dir, 0.5f, false);
    }
    public virtual void  AI_Doing_Jump()
    {
        rigid.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    public virtual void AI_Doing_JumpAfterMove()
    {
        // ���� �� ���ݿ��� ���
        Vector3 heading = (this.transform.position - target.transform.position).normalized; // ĳ���� - ��(��) �� �ǵ���
        Vector3 dir = target.transform.position + heading;
        this.transform.DOMove(dir, 0.8f).SetEase(Ease.InQuad);

        // ������ ����Ʈ ���
    }
    public virtual void  AI_Doing_Rolling()
    {
        float duration = 0.5f;
        
        model.DOLocalRotate(new Vector3(0, 360, 0), duration * 0.5f , RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(2, LoopType.Incremental);
        Vector3 heading = (this.transform.position - target.transform.position).normalized; // ĳ���� - ��(��) �� �ǵ���
        Vector3 dir = target.transform.position + heading;
        this.transform.DOMove(dir, duration, false).SetEase(Ease.InQuad);
    }
}
