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
        // �˾� �ҷ���
       private readonly List<FlowNode> _listNode = new List<FlowNode>(); 
        public PopupStyle CurStyle { get; private set; } = PopupStyle.None;

        // = Get
        public FlowNode AddFlow(PopupStyle style, params object[] data)
        {
            // ���ο� ��� �ְ� ��ȯ��Ŵ
            Push(new FlowNode(style, data));
            return GetLastNode();
        }
        public FlowNode GetLastNode()
        {
            // ���� FlowNode�� ������ 1�̻��̶�� ������ -1(�迭�̴ϱ�) ��ȯ, 0�̸� null ��ȯ
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
            AllHideSubPopup(true); // �ٸ��� �� �����
            CurStyle = style;

            if (style == PopupStyle.None)
                return;

            AddFlow(style, data);
            PopupManager.Instance.Show<PopupBase>(style, data).Subscribe();
        }
        /*
        #region ::: Change IObservable :::
        public IObservable<T> Change<T>(PopupStyle style, params object[] data) where T : PopupBase // (����ϴ��� Ȯ��ġ����)
        {
            if (CurStyle == style && style != PopupStyle.None)
                return NonePopupObservable<T>(); // Change�� ����ҋ� ������ <T> ���� �״�� ��ȯ
            var node = GetLastNode(); // ���� ������(�������) ��� ����akwlakr����

            // LastNode�� �޾ƿ� node�� �����Ѵٸ� style�� ����� ���ٸ� ���� curStyle �� �����.
            PopupManager.Instance.Hide(node != null ? node.Style : CurStyle);
            AllHideSubPopup(true);
            CurStyle = style;

            if (style == PopupStyle.None)
                return NonePopupObservable<T>();

            AddFlow(style, data); // Flow �� �߰���
            return PopupManager.Instance.Show<T>(style, data);
        }
        private IObservable<T> NonePopupObservable<T>() where T : PopupBase
        {
            var observer = new Subject<T>(); // Subject�� ���ø� ���·� ������ observer ����
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
                    subPopup.Hide(); // ���� �˾��� �����
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
        private void Push(FlowNode nextNode) // ������ ��带 �Ķ���ͷ� ����
        {
            if (nextNode == null)
            {
                _listNode.Clear();
                return;
            }
            int index = _listNode.FindIndex(node=> node.Style.Equals(nextNode.Style)); // FindIndex ���� : startIndex, count, predicated
            if(index != -1) // -1�� ������ ���µ� ��������
            {
                _listNode.RemoveRange(index, (_listNode.Count - index));
            }
            _listNode.Add(nextNode);
        }
    }

  


    // =SubClass
    public class FlowNode
    {
        public PopupStyle Style { get; private set; } // �ܺο��� �����ϸ� �ȴ�
        public object[] Data { get; set; }
        public FlowNode(PopupStyle style, params object[] data)
        {
            Style = style;
            if(data != null && data.Length > 0)
            {
                if (data[0] != null)
                {
                    Data = null; // �Ķ������ data���� ���ٸ� FlowNode�������� Data�� null��
                }
            }
        }

    }
}