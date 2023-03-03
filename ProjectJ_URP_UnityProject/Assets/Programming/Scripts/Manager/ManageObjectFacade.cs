using Assets.Scripts.Manager;
using Assets.Scripts.UI;
using System;
using System.Collections;
using UniRx;
using System.Threading;

namespace Assets.Scripts.MangeObject
{
    public class ManageObjectFacade
    {
        private static bool _isInitialize = false;

        public static FlowManager FlowManager
        { get { return FlowManager.Instance; } }

        public static PopupManager PopupManager
        { get { return PopupManager.Instance; } }

        public static void Initialize()
        {
            if (_isInitialize)
                return;
            DebugManager.ins.Log("ManagerCreate", DebugManager.TextColor.Yellow);
            _isInitialize = true;
            FlowManager.Initialize();
            PopupManager.Initialize();
        }

        public static IObservable<Unit> UnInitialize()
        {
            return Observable.FromCoroutine<Unit>(UnInitialize);
        }
        public static IEnumerator UnInitialize(IObserver<Unit> observer, CancellationToken token)
        {
            if(_isInitialize == false)
            {
                DebugManager.ins.Log("UnInitialize");
                observer.OnNext(Unit.Default);
                observer.OnCompleted();
            }
            else
            {
                PopupManager.UnInitialize();
                FlowManager.UnInitialize();
                yield return FrameCountType.FixedUpdate.GetYieldInstruction();
                _isInitialize = false;
                observer.OnNext(Unit.Default);
                observer.OnCompleted();
            }
        }

    }
}