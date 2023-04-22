using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfElseActionNode : ILeafNode
{
    // 조건 참일 경우 IfAction 수행 및 true 리턴
    // 조건 거짓일 경우 ElseAction 수행 및 false 리턴
    /// <summary>
    /// 조건에 따른 수행 노드
    /// </summary>
    
    
    public Func<bool> Condition { get; }
    public Action IfAction { get; }
    public Action ElseAction { get; }

    public IfElseActionNode(Func<bool> condition, Action ifAction, Action elseAction)
    {
        Condition = condition;
        IfAction = ifAction;
        ElseAction = elseAction;
    }

    public bool Run() // 동작 결과에 라 If / Else Action 후 결과 반환
    {
        bool result = Condition();

        if (result) IfAction();
        else ElseAction();

        return result;
    }
}
