using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 노드 사용을 도움
public static class NodeHelper
{
    public static SelectorNode Selector(params INode[] nodes) => new SelectorNode(nodes);
    
    public static IfActionNode IfAction(Func<bool> condition, Action action)
        => new IfActionNode(condition, action);

    public static IfElseActionNode IfElseAction(Func<bool> condition, Action ifAction, Action ifElseAction)
        => new IfElseActionNode(condition, ifAction, ifElseAction);
}
