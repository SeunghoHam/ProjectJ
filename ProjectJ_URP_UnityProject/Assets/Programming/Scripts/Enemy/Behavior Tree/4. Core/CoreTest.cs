using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static  NodeHelper;
public class CoreTest : MonoBehaviour, ICore
{
    private INode _rootNode;

    private void Awake()
    {
        MakeNode();
    }

    private void FixedUpdate()
    {
        _rootNode.Run();
    }

    private void MakeNode()
    {
        _rootNode =
            Selector
            (
                IfAction(Test1,TestAction1),
                IfAction(Test2, TestAction2)
            );
    }

    private bool Test1()
    {
        return true;
    }

    private bool Test2()
    {
        return false;
    }

    private void TestAction1() => Debug.Log("대기상태");
    private void TestAction2() => Debug.Log("액션");
}
