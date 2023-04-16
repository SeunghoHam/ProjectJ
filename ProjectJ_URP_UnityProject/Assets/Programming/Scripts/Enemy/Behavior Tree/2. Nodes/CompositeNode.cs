using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Abstract Class 로 제작
public abstract class CompositeNode : ICompositeNode
{
    /// <summary> 자식 순환 노드 </summary>
    public List<INode> ChildList { get; protected set; }
    
    // 생성자
    public CompositeNode(params INode[] nodes) => ChildList = new List<INode>(nodes); 
    
    // 자식 노드 추가
    public CompositeNode Add(INode node)
    {
        ChildList.Add(node);
        return this; // List에 추가하고 자기 반환
    }

    public abstract bool Run();
}
