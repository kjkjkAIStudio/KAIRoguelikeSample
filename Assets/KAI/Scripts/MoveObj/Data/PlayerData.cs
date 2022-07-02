using KAI.Mgr;

namespace KAI
{
    namespace MoveObjN
    {
        [System.Serializable]
        public class PlayerData : Data
        {
            public int maxHP;
            public float speed;
            public string[] startWeapon;
        }
    }
}