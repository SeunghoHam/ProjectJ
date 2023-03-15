using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.Manager;
using UniRx.Triggers;

public class EnemyMovement : MonoBehaviour
{
    // 움직임 및 회전 관련

    private Transform model; // 회전될 오브젝트
    [SerializeField] private Transform target; // 목적 대상 ( 스크립트에서 받아올 수 있게 해야함)
    private EnemyAnimator animator;

    private Rigidbody rigid;
    private bool isDoing = false; // 동작중
    private float _jumpForce = 3f;
    private float _moveDuration = 0.5f; // 이동에 걸리는 시간
    private void Awake()
    {
        model = this.transform.GetChild(0).transform;
        rigid = this.GetComponent<Rigidbody>();
        //target = Character.Instance.gameObject.transform;

        
    }
    public void Initialize(EnemyAnimator Anim) // Enemy 스크립트에서 필요한 정보들 가져오기
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

    public void AI_Doing_Avoid() // 플레이어가 그냥 공격하면 피해버림
    {
        // 플레이어의 위치 - 나(적)의 위치 = 이동해야할 방향의 벡터를 반환

        // 방향구하기
        // 방향 벡터값 = 목표 벡터값 - 시작벡터
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
