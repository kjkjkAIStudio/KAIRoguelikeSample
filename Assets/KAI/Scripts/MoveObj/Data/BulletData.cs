using UnityEngine;
using KAI.Mgr;

namespace KAI
{
    namespace MoveObjN
    {
        [System.Serializable]
        public class BulletData : Data
        {
            public int damage;
            public string resName, resHitName;
            public int frameRate;
            public int resFrameCount, resHitFrameCount;

            [System.Serializable]
            public class TimeLine
            {
                public float time;
                public float speed, acc;
                public float rotSpeed;
                public Vector2 sclSpeed;
                public bool rndSpeed;
                public float rndSpeedMin, rndSpeedMax;
                public bool rndAcc;
                public float rndAccMin, rndAccMax;
                public bool rndRotSpeed;
                public float rndRotSpeedMin, rndRotSpeedMax;
                public bool rndSclSpeed;
                public Vector2 rndSclSpeedMin, rndSclSpeedMax;
                public bool aimPlayer;
            }
            public TimeLine[] timeLine;
        }
    }
}