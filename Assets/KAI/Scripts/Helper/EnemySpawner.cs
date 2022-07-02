using UnityEngine;
using KAI.MoveObjN;

namespace KAI
{
    namespace Helper
    {
        public class EnemySpawner
        {
            public bool IsSpawn { get; set; }
            public int MaxEnemyCount { get; set; }

            public EnemySpawner()
            {

            }

            public void Update()
            {
                if (IsSpawn)
                {
                    foreach (Spawner i in spawner)
                    {
                        i.time += Time.deltaTime;
                        if (i.time > i.delay)
                        {
                            i.SetSpawnerDelay();
                            if (Core.Inst.GM.EnemyDict.Count < MaxEnemyCount)
                                i.SpawnEnemy();
                        }
                    }
                }
            }

            public void SetData(Transform t_spawner)
            {
                spawner = new Spawner[t_spawner.childCount];
                for (int i = 0; i < spawner.Length; ++i)
                {
                    Mono_enemySpawnerData monoData = t_spawner.GetChild(i).GetComponent<Mono_enemySpawnerData>();
                    spawner[i] = new Spawner(new Data(monoData.transform.position, monoData.minDelay, monoData.maxDelay));
                }
            }

            private class Data
            {
                public Vector3 pos;
                public float minDelay, maxDelay;

                public Data(Vector3 pos, float minDelay, float maxDelay)
                {
                    this.pos = pos;
                    this.minDelay = minDelay;
                    this.maxDelay = maxDelay;
                }
            }
            
            private class Spawner
            {
                public Data Data { get; private set; }
                public float time, delay;

                public Spawner(Data data)
                {
                    Data = data;
                    SetSpawnerDelay();
                }

                public void SetSpawnerDelay()
                {
                    time = 0.0f;
                    delay = Random.Range(Data.minDelay, Data.maxDelay);
                }

                public void SpawnEnemy()
                {
                    string enemyName = EnemyList[Random.Range(0, EnemyList.Length)];
                    Enemy enemy = new Enemy(Core.Inst.DM.GetEnemy(enemyName));
                    Core.Inst.GM.EnemyDict.Add(enemy);
                    enemy.TP(Data.pos);
                    enemy.BS.Shoot();
                    enemy.OnQueryDestroy += (inst) =>
                    {
                        inst.Destroy();
                        Core.Inst.GM.EnemyDict.Remove(inst);
                    };
                }

                private readonly string[] EnemyList =
                {
                    "enemy_0", "enemy_1", "enemy_2"
                };
            }

            private Spawner[] spawner;
        }
    }
}