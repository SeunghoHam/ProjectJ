using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Threading;
using System.Linq;

namespace Assets.Scripts.UI
{
    public class PopupManager : UnitySingleton<PopupManager>
    {
        // = Constant
        private const string PopupPrefix = "Prefs/Popup/UIPopup";

        // = Field
        public readonly List<PopupBase> PopupList = new List<PopupBase>();

        public override void Initialize()
        {
            base.Initialize();
        }
        public override void UnInitialize()
        {
            //base.UnInitialize();
            foreach(var popup in PopupList) 
            {
                Destroy(popup.gameObject);
            }
            PopupList.Clear();
        }

        // = Construct
        public IObservable<T> Get<T>(PopupStyle style) where T : PopupBase
        {
            return Observable.FromCoroutine<T>((observer, token) => Get(observer, token, style));
        }
        private IEnumerator Get<T>(IObserver<T> observer, CancellationToken cancelltationToken, PopupStyle style) where T : PopupBase
        {
            PopupBase popupBase = null;
            yield return Observable.FromCoroutineValue<T>(() => 
            Get<T>(cancelltationToken, style))
                .Where(popup => popup != null)
                .StartAsCoroutine(popup => popupBase = popup);
            observer.OnNext(popupBase.GetComponent<T>());
            observer.OnCompleted();
        }
        private IEnumerator Get<T>(CancellationToken token, PopupStyle style) where T : PopupBase
        {
            var popupName = GetPopupName(style);
            var popupBase = PopupList.Find(child => child.PopupStyle.Equals(style));

            if(popupBase == null)
            {
                var resource = Resources.LoadAsync<GameObject>(popupName);
                while(!resource.isDone)
                {
                    if (token.IsCancellationRequested)
                        yield break;
                    yield return FrameCountType.FixedUpdate.GetYieldInstruction();
                }
                // = resources로드 끝
                popupBase = Instantiate((GameObject)resource.asset).GetComponent<PopupBase>();

                // 하위로 내려가며 가장 가까운 위치의 canvas 가져옴
                var popupCanvas = popupBase.GetComponentInChildren<Canvas>();
                if(popupBase as PopupSub)
                {
                    // 카메라 할당 X 
                    popupCanvas.worldCamera = null;
                    popupCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                }
                else
                {
                    // 카메라 할당 O
                    // 카메라 매니저 따로 안만들고 그냥 메인캠 할당시킴

                    //popupCanvas.worldCamera = Camera.main; 
                    //popupCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                    popupCanvas.worldCamera = null;
                    popupCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                }
                popupBase.transform.SetParent(transform);
                popupBase.transform.localScale = Vector3.one;
                popupBase.transform.localPosition = Vector3.zero;
                popupBase.gameObject.SetActive(false);
                PopupList.Add(popupBase);
            }
            yield return popupBase;
        }

        // Show
        public IObservable<T> Show<T> (PopupStyle style, params object[] data) where T: PopupBase
        {
            return Observable.FromCoroutine<T>((observer,token)=>Show(observer,token,style,data)); // 인자가 4개인 Show 불러오기
        }
        private IEnumerator Show<T>(IObserver<T> observer, CancellationToken token, PopupStyle style, object[] data) where T: PopupBase 
        {
            yield return Get<T>(token, style);

            PopupBase popupBase = PopupList[0];

            popupBase.SetParent(transform);
            popupBase.Show(data);
            popupBase.gameObject.SetActive(true);

            observer.OnNext(popupBase.GetComponent<T>());
            observer.OnCompleted();
        }

        public void Hide(PopupStyle style)
        {
            var popup = PopupList.Find(child => child.PopupStyle.Equals(style));
            if (popup != null && popup.gameObject.activeSelf)
                popup.Hide();
        }
        // = Method
        private string GetPopupName(PopupStyle style)
        {
            // Prefs/Popup/UIPopup  + style
            return string.Format("{0}{1}", PopupPrefix, style); 
        }
        public List<PopupBase> GetShowingPopupList()
        {
            Debug.Log("GetShowingPopupList");
            return PopupList.Where(@base => @base.IsActive).ToList();
        }
    }
    
}
