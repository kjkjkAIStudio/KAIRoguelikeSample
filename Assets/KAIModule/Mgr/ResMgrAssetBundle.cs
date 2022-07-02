using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KAIModule
{
    namespace Mgr
    {
        public class ResMgrAssetBundle : IResMgr
        {
            public ResMgrAssetBundle()
            {
                abDict = new Dictionary<string, AssetBundle>();
            }

            public void Open(string pack)
            {
                if (abDict.ContainsKey(pack))
                    return;
                AssetBundle ab = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, pack));
                if (ab != null)
                    abDict.Add(pack, ab);
            }

            public void Close(string pack)
            {
                if (abDict.TryGetValue(pack, out AssetBundle ab))
                {
                    ab.Unload(false);
                    abDict.Remove(pack);
                }
            }

            public T Get<T>(string pack, string asset) where T : Object
            {
                if (abDict.TryGetValue(pack, out AssetBundle ab))
                {
                    return ab.LoadAsset<T>(asset);
                }
                else
                {
                    Debug.LogErrorFormat("[ResMgrAssetBundle] No pack called \"{0}\" is open.", pack);
                    return null;
                }
            }

            public T Get<T>(string pack, string asset, int index) where T : Object
            {
                if (abDict.TryGetValue(pack, out AssetBundle ab))
                {
                    return ab.LoadAssetWithSubAssets<T>(asset)[index];
                }
                else
                {
                    Debug.LogErrorFormat("[ResMgrAssetBundle] No pack called \"{0}\" is open.", pack);
                    return null;
                }
            }

            private Dictionary<string, AssetBundle> abDict;
        }
    }
}