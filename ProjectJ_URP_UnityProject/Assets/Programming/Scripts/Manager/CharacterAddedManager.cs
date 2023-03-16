using UnityEngine;
using Assets.Scripts.Common.DI;
using Assets.Scripts.Manager;
using Unity.VisualScripting;

public class CharacterAddedManager : MonoBehaviour
{
    [DependuncyInjection(typeof(ResourcesManager))]
    ResourcesManager ResourcesManager;

    private const string filePath = "Prefabs/System/";
    private const string _cameraSystem = "#CameraSystem";
    private const string _manager = "#Manager";
    //private const string _character = "Iroha_Model"; //  Graphic_Resources로 이동함
    [SerializeField] private GameObject iroha_modelObject;

    private Transform _createPos;
    private void Awake()
    {
        _createPos = this.gameObject.transform.GetChild(0);
        //GameObject character =  Instantiate(ResourcesManager.Load(filePath + _character));

        if (iroha_modelObject != null)
        {
            GameObject character = Instantiate(iroha_modelObject, this.transform);
            character.transform.position = _createPos.position;
        }
        else
        {
            DebugManager.ins.Log("캐릭터 프리팹이 할당되지 않음 : iroha_modelObjet", DebugManager.TextColor.Red);
        }

        var cam = Instantiate(ResourcesManager.Load(filePath + _cameraSystem));
        var manager = Instantiate(ResourcesManager.Load(filePath + _manager));

        
        cam.transform.parent = transform;
        manager.transform.parent = transform;

        Character.Instance.cameraSystem = cam.GetComponent<CameraSystem>();
    }
}
