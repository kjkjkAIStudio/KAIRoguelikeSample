using System.IO;
using UnityEngine;
using UnityEditor;

namespace KAIModule
{
    namespace Editor
    {
        public class GenAB
        {
            [MenuItem("KAI/生成AB")]
            public static void Gen()
            {
                if (!Directory.Exists(Application.streamingAssetsPath))
                    Directory.CreateDirectory(Application.streamingAssetsPath);

#if UNITY_STANDALONE_WIN
                BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
#elif UNITY_ANDROID
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
#endif
                AssetDatabase.Refresh();
            }
        }
    }
}