using UnityEngine;

using Assets.Scripts.Common.DI;
using Assets.Scripts.Manager;

public class CharacterAddedManager : MonoBehaviour
{
    [DependuncyInjection(typeof(ResourcesManager))]
    ResourcesManager ResourcesManager;

    private const string filePath = "Prefs/Added/";
    private const string _cameraSystem = "#CameraSystem";
    private const string _manager = "#Manager";
    private const string _character = "Iroha_Model";

    private Transform _createPos;
    private void Awake()
    {
        _createPos = this.gameObject.transform.GetChild(0);
        GameObject character =  Instantiate(ResourcesManager.Load(filePath + _character));
        var cam = Instantiate(ResourcesManager.Load(filePath + _cameraSystem));
        var manager = Instantiate(ResourcesManager.Load(filePath + _manager));

        character.transform.position = _createPos.position;
        cam.transform.parent = transform;
        manager.transform.parent = transform;
    }
}
