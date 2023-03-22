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
        // 팝업 불러옴
       private readonly List<FlowNode> _listNode = new List<FlowNode>(); 
        public PopupStyle CurStyle { get; private set; } = PopupStyle.None;

        // = Get
        public FlowNode AddFlow(PopupStyle style, params object[] data)
        {
            // 새로운 노드 넣고 반환시킴
            Push(new FlowNode(style, data));
            return GetLastNode();
        }
        public FlowNode GetLastNode()
        {
            // 현재 FlowNode의 개수가 1이상이라면 개수의 -1(배열이니까) 반환, 0이면 null 반환
            return (_listNode.Count > 0 ? _listNode[_listNode.Count - 1] : null);
        }
        #region ::: Add Sub Popup :::
        public void AddSubPopup(PopupStyle style, params object[] data) //
        {
            PopupManager.Instance.Show<PopupSub>(style, data).Subscribe();
        }
        public IObservable<T> AddSubPopup<T>(PopupStyle style, params object[] data) where T : PopupSub
        {
            return PopupManager.Instance.Show<T>(style, data);
        }
        #endregion



        public void Change(PopupStyle style, params object[] data) // void : Change
        {
            if (CurStyle == style && style != PopupStyle.None)
                return;

            var node = GetLastNode();
            PopupManager.Instance.Hide(node != null ? node.Style : CurStyle);
            AllHideSubPopup(true); // 다른거 다 지우기
            CurStyle = style;

            if (style == PopupStyle.None)
                return;

            AddFlow(style, data);
            PopupManager.Instance.Show<PopupBase>(style, data).Subscribe();
        }
        /*
        #region ::: Change IObservable :::
        public IObservable<T> Change<T>(PopupStyle style, params object[] data) where T : PopupBase // (사용하는지 확실치않음)
        {
            if (CurStyle == style && style != PopupStyle.None)
                return NonePopupObservable<T>(); // Change를 사용할떄 가져온 <T> 형태 그대로 반환
            var node = GetLastNode(); // 현재 마지막(사용중인) 노드 가져akwlakr오기

            // LastNode로 받아온 node가 존재한다면 style를 지우고 없다면 현재 curStyle 를 지운다.
            PopupManager.Instance.Hide(node != null ? node.Style : CurStyle);
            AllHideSubPopup(true);
            CurStyle = style;

            if (style == PopupStyle.None)
                return NonePopupObservable<T>();

            AddFlow(style, data); // Flow 에 추가함
            return PopupManager.Instance.Show<T>(style, data);
        }
        private IObservable<T> NonePopupObservable<T>() where T : PopupBase
        {
            var observer = new Subject<T>(); // Subject를 탬플릿 형태로 선언한 observer 선언
            Observable.NextFrame().Subscribe(_ =>
            {
                observer.OnNext(null);
                observer.OnCompleted();
            }).AddTo(gameObject);
            return observer;
        }
        #endregion*/

        private void AllHideSubPopup(bool isForce)
        {
            var popupList = PopupManager.Instance.GetShowingPopupList();
            foreach (var subPopup in popupList)
            {
                subPopup.Hide();
                
                if (isForce)
                {
                    subPopup.Hide(); // 보조 팝업들 지우기
                }
                else
                {
                    if(!subPopup.IsIgnoreEscapeHide)
                    {
                        subPopup.Hide();
                    }
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
            int index = _listNode.FindIndex(node=> node.Style.Equals(nextNode.Style)); // FindIndex 인자 : startIndex, count, predicated
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