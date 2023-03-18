using Assets.Scripts.Common;
using Assets.Scripts.UI;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace Assets.Scripts.Manager
{
    public class FlowManager : UnitySingleton<FlowManager>
    {
       private readonly List<FlowNode> _listNode = new List<FlowNode>();
        public PopupStyle CurStyle { get; private set; } = PopupStyle.None;

        // = Get
        public FlowNode AddFlow(PopupStyle style, params object[] data)
        {
            Push(new FlowNode(style, data));
            return GetLastNode();
        }
        public FlowNode GetLastNode()
        {
            // 현재 FlowNode의 개수가 1이상이라면 개수의 -1(배열이니까) 반환, 0이면 null 반환
            return (_listNode.Count > 0 ? _listNode[_listNode.Count - 1] : null);
        }

        
        public IObservable<T> AddSubPopup<T>(PopupStyle style, params object[] data) where T : PopupSub
        {
            return PopupManager.Instance.Show<T>(style, data);
        }
        public void AddSubPopup(PopupStyle style, params object[] data) //
        {
            PopupManager.Instance.Show<PopupSub>(style, data).Subscribe();
        }
        
        public void Change(PopupStyle style, params object[] data) // void : Change
        {
            if (CurStyle == style && style != PopupStyle.None)
                return;

            var node = GetLastNode();
            PopupManager.Instance.Hide(node != null ? node.Style : CurStyle);
            AllHideSubPopup(true);
            CurStyle = style;

            if (style == PopupStyle.None)
                return;

            AddFlow(style, data);
            PopupManager.Instance.Show<PopupBase>(style, data).Subscribe();
        }
        private void AllHideSubPopup(bool isForce)
        {
            var popupList = PopupManager.Instance.GetShowingPopupList();
            foreach (var subPopup in popupList)
            {
                if (isForce)
                {
                    subPopup.Hide(); // 보조 팝업들 지우기
                }
                else
                {
                    /*
                    if (!subPopup.isignore)
                    {
                        subPopup.Hide();
                    }*/
                }
            }
        }
        // = Method
        private void Push(FlowNode nextNode) // 생성될 노드를 파라미터로 받음
        {
            if (nextNode == null)
            {
                _listNode.Clear();
                return;
            }
            int index = _listNode.FindIndex(node=> node.Style.Equals(nextNode.Style));
            if(index != -1) // -1가 될일이 없는데 오류방지
            {
                _listNode.RemoveRange(index, (_listNode.Count - index));
            }
            _listNode.Add(nextNode);
        }
    }

  


    // =SubClass
    public class FlowNode
    {
        public PopupStyle Style { get; private set; } // 외부에서 수정하면 안댐
        public object[] Data { get; set; }
        public FlowNode(PopupStyle style, params object[] data)
        {
            Style = style;
            if(data != null && data.Length > 0)
            {
                if (data[0] != null)
                {
                    Data = null; // 파라미터의 data값이 없다면 FlowNode지역변수 Data도 null로
                }
            }
        }

    }
}