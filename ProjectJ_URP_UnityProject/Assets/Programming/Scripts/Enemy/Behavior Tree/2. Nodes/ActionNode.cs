using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : ILeafNode
{
    /// <summary> 행동 수행 노드</summary>
    public Action Action { get; protected set; }

    // 생성자로 Action 할당
    public ActionNode(Action action)
    {
        Action = action;
    }
    // ILeafNode : INode 를 상속받아서 Run() 함수를 정의해야함
    public virtual bool Run()
    {
        Action(); // 액션 실행
        return true; // 무조건 true 반환
    }
    
    
    // : staitc implicit operator 란...
    // C#에서 static implicit operator는 클래스나 구조체 등의 사용자 정의 형식을 다른 형식으로 변환하기 위해 사용됩니다.
    // 이 연산자를 정의하면 컴파일러가 사용자 정의 형식의 인스턴스를 다른 형식의 인스턴스로 자동으로 변환할 수 있습니다.
    
    // Action <=> ActionNode 타입 캐스팅
    public static implicit operator ActionNode(Action action) => new ActionNode(action);
    public static implicit operator Action(ActionNode action) => new Action(action.Action); // 파라미터인 액션노드의 액션을 반환
}
