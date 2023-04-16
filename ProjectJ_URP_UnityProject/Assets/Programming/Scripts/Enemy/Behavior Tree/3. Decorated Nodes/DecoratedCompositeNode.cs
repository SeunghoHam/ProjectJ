using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecoratedCompositeNode : CompositeNode
{
    /// <summary>조건에 따른 Composite(순환) 수행 노드 </summary>
    public Func<bool> Condition { get; }
    public CompositeNode Composite { get; }

    public DecoratedCompositeNode(Func<bool> condition, CompositeNode composite)
    {
        Condition = condition;
        Composite = composite;
    }
    
    public override bool Run()
    {
        if (!Condition())
            return false;
        
        return Composite.Run(); // 조건이 되지 않으면 바로 false 반환. 조건이 가능하면 순환 노드를 진행함
    }
}
