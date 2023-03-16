using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    /// <summary> 피격당함 </summary>
    public virtual void Damaged(int damage) { } // 피격됨
    public virtual void Attack() { }
    public virtual void Avoid() { } // 캐릭터 - 구르기, 보스 - 회피기
    public virtual void Death() { }

}
