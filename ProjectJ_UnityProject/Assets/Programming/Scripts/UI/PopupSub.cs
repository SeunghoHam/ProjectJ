using UnityEngine;

namespace Assets.Scripts.UI
{
    public class PopupSub : PopupBase
    {
        protected bool BackgroundHideCheckLocked { get; set; }
        public virtual void SetHideCheckTransform(Transform hideCheckTransform)
        {

        }
        protected override void OnDestroy()
        {
            Hide();
            UnInitialize();
            //base.OnDestroy();
        }
    }
}