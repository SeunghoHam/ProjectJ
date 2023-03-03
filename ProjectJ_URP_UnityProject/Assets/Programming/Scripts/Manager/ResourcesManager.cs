using Assets.Scripts.Common;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class ResourcesManager : UnitySingleton<ResourcesManager>
    {
        private Dictionary<string, Sprite> _imageCaches = new Dictionary<string, Sprite>();
        private Dictionary<string, GameObject> _objectCaches = new Dictionary<string, GameObject>();
        public override void Initialize()
        {
            base.Initialize();
        }

        #region ###프리팹 로드
        public static GameObject LoadAndInit(string path, Transform parent)
        {
            var pathLoad = Load(path);
            if(pathLoad == null)
            {
                DebugManager.ins.LogError("LoadAndInit Error : " + path);
                return null;
            }
            var item = Instantiate(pathLoad, parent);
            var transform = item.GetComponent<Transform>();
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            return item;
        }

        public static GameObject Load(string path)
        {
            return Resources.Load<GameObject>(path);
        }
        #endregion

        #region ###이미지 로드
        public static Sprite GetImages(string id)
        {
            if(!Instance._imageCaches.ContainsKey(id))
            {
                DebugManager.ins.LogError("GetImages Erorr : " + id);
                return null;
            }
            return Instance._imageCaches[id];
        }

        public static Sprite GetPathImage(string subPath, string name)
        {
            if(Instance._imageCaches.ContainsKey(name) == false)
            {
                var path = string.Format("{0}{1}", subPath, name);
                var sprite = Resources.Load<Sprite>(path);
                Instance._imageCaches.Add(name, sprite); // 생성한 이미지의 이름으로 캐시 추가
            }
            return Instance._imageCaches[name];
        }
        #endregion
    }
}