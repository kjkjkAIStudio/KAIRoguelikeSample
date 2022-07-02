using UnityEngine;
using UnityEngine.SceneManagement;

namespace KAI
{
    namespace Mgr
    {
        public class StageMgr
        {
            public StageMgr()
            {

            }

            public void ToTitle(System.Action<Mono_Title> callback)
            {
                SceneManager.LoadSceneAsync("Title").completed += (p) => { callback?.Invoke(Object.FindObjectOfType<Mono_Title>()); };
            }

            public void ToStage(int index, System.Action<Mono_Stage> callback)
            {
                if (index >= StageCount)
                {
                    callback?.Invoke(null);
                    return;
                }
                string sceneName = string.Format("Stage_{0}", index);
                SceneManager.LoadSceneAsync(sceneName).completed += (p) => { callback?.Invoke(Object.FindObjectOfType<Mono_Stage>()); };
            }

            private const int StageCount = 2;
        }
    }
}