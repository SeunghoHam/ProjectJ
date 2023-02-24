using Assets.Scripts.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class PopupBase : MonoBehaviour
    {
        public bool DontDestory;
        private PopupStyle _popupStyle;
        public PopupStyle PopupStyle
        {
            get
            {
                if (!PopupStyle.Equals(PopupStyle.None))
                    return _popupStyle;

                var popupName = gameObject.name;
                popupName = popupName.Replace("UIPopup", "");
                popupName = popupName.Replace("(Clone)", "");
                try
                {
                    _popupStyle = (PopupStyle)System.Enum.Parse(typeof(PopupStyle), popupName);
                }
                catch (Exception)
                {
                    _popupStyle = PopupStyle.None;
                }
                return _popupStyle;
            }
        }

        protected CompositeDisposable CancelerObject; // �̺�Ʈ�� �ѹ��� UnSubcribe(Destory)��Ű��
        // �ڵ� ������ ����Ŭ�� ���߼� �ѹ��� ��� �޸� ���� ����

        public bool IsActive
        {
            get { return gameObject.activeInHierarchy; }
        }
        private void Awake()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
        }
        public virtual void UnInitialize()
        {
        }

        public virtual void Show(params object[] data)
        {
            if (CancelerObject != null)
                CancelerObject.Dispose();
            DebugManager.ins.Log("PouppBase Show : " + gameObject.name, DebugManager.TextColor.Blue);
            CancelerObject = new CompositeDisposable();
            gameObject.SetActive(true); // Ȱ��ȭ��Ŵ
        }
        public virtual void Hide()
        {
            if(CancelerObject != null)
            {
                CancelerObject.Dispose();
                CancelerObject = null;
            }
            if (gameObject != null)
                gameObject.SetActive(false);
            if (DontDestory)
                return;
            Destroy();
        }
        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
        }

        private void Destroy()
        {
            if (PopupManager.Instance)
                PopupManager.Instance.PopupList.Remove(this);
            Destroy(gameObject);
        }
        protected virtual void OnDestroy()
        {
            Hide();
            UnInitialize();
            Resources.UnloadUnusedAssets();
        }
          
    }
}