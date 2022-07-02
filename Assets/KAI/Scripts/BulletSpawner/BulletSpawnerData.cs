using KAI.Mgr;

namespace KAI
{
    namespace BulletSpawnerN
    {
        [System.Serializable]
        public class BulletSpawnerData : Data
        {
            [System.Serializable]
            public class TimeLine
            {
                public float time;
                public string bullet;
                public int count;
                public float rot;
                public float deltaRot;
                public bool rndRot;
                public float rndRotMin, rndRotMax;
                public bool aimPlayer;
            }
            public TimeLine[] timeLine;
        }
    }
}