using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    /// <summary> �ǰݴ��� </summary>
    public virtual void Damaged(int damage) { } // �ǰݵ�
    public virtual void Attack() { }
    public virtual void Avoid() { } // ĳ���� - ������, ���� - ȸ�Ǳ�
    public virtual void Death() { }

}
