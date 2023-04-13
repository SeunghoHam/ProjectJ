using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Animator _iroha_Sword;

    public void SwordAnim_Show()
    {
        _iroha_Sword.SetTrigger("Show");
    }
    public void SwordAnim_Hide()
    {
        _iroha_Sword.SetTrigger("Hide");
    }

    public void TimerStart()
    {
        //var mouseUpStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)); // ���콺 Ŭ���� ������ Hdie ��� �� �ٽ� �����ǵ���
        Observable.Timer(TimeSpan.FromSeconds(1.5f))
            .Take(1)
            //.Repeat()
            //.RepeatUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                //Debug.Log("Į �������");
                SwordAnim_Hide();
            });

    }
}
