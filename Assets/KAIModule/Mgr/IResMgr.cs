using UnityEngine;

namespace KAIModule
{
    namespace Mgr
    {
        public interface IResMgr
        {
            void Open(string pack);
            void Close(string pack);
            T Get<T>(string pack, string asset) where T : Object;
            T Get<T>(string pack, string asset, int index) where T : Object;
        }
    }
}