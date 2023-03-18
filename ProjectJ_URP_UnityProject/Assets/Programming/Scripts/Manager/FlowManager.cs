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
            // ���� FlowNode�� ������ 1�̻��̶�� ������ -1(�迭�̴ϱ�) ��ȯ, 0�̸� null ��ȯ
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
                    subPopup.Hide(); // ���� �˾��� �����
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
        private void Push(FlowNode nextNode) // ������ ��带 �Ķ���ͷ� ����
        {
            if (nextNode == null)
            {
                _listNode.Clear();
                return;
            }
            int index = _listNode.FindIndex(node=> node.Style.Equals(nextNode.Style));
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