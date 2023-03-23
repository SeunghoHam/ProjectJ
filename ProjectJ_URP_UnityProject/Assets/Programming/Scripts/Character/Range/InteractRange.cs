using Assets.Scripts.Manager;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Popup.Base;
using UnityEngine;

public class InteractRange : RangeSystem
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interact"))
        {
            Character.Instance.CanInteract = true;
            PopupManager.Instance.PopupList[0].GetComponent<UIPopupBasic>()._basicView.IntearctActive(true);
        }
        else return;
    }
    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interact"))
        {
            Character.Instance.CanInteract = false;
            PopupManager.Instance.PopupList[0].GetComponent<UIPopupBasic>()._basicView.IntearctActive(false);
        }
        else return;
    }
}
