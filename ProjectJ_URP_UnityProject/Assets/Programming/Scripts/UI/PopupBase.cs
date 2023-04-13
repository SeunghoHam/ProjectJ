using System;
using UniRx;
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
                if (!_popupStyle.Equals(PopupStyle.None))
                    return _popupStyle;

                var popupName = gameObject.name; // ȣ�� ����� �̸� ������
                popupName = popupName.Replace("UIPopup", "");
                popupName = popupName.Replace("(Clone)", "");
                try
                {
                    _popupStyle = (PopupStyle)Enum.Parse(typeof(PopupStyle), popupName);
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
        public bool IsIgnoreEscapeHide { get; set; }
        private void Awake()
        {
            Initialize();
        }

        public virtual void Initialize()
        { }
        public virtual void UnInitialize()
        { }

        public virtual void Show(params object[] data)
        {
            DebugManager.ins.Log("PouppBase Show : " + gameObject.name, DebugManager.TextColor.Blue);
            if (CancelerObject != null)
                CancelerObject.Dispose();
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