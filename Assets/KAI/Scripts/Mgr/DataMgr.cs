using System.Collections.Generic;
using UnityEngine;
using KAI.MoveObjN;
using KAI.BulletSpawnerN;
using KAI.WeaponN;
using KAIModule.Mgr;

namespace KAI
{
    namespace Mgr
    {
        public class DataMgr
        {
            public DataMgr(IResMgr rm)
            {
                rm.Open("data");
                playerData = LoadSingle<PlayerData>(rm, "char");
                enemyData = LoadSingle<EnemyData>(rm, "enemy");
                bulletData = LoadSingle<BulletData>(rm, "bullet");
                bulletSpawnerData = LoadSingle<BulletSpawnerData>(rm, "bulletSpawner");
                weaponData = LoadSingle<WeaponData>(rm, "weapon");
                rm.Close("data");
            }

            public PlayerData GetPlayer(string name)
            {
                return GetSingle(playerData, name);
            }

            public EnemyData GetEnemy(string name)
            {
                return GetSingle(enemyData, name);
            }

            public BulletData GetBullet(string name)
            {
                return GetSingle(bulletData, name);
            }

            public BulletSpawnerData GetBS(string name)
            {
                return GetSingle(bulletSpawnerData, name);
            }

            public WeaponData GetWeapon(string name)
            {
                return GetSingle(weaponData, name);
            }

            [System.Serializable]
            private class DataArray<T> where T : Data
            {
                public T[] array;
            }

            private Dictionary<string, PlayerData> playerData;
            private Dictionary<string, EnemyData> enemyData;
            private Dictionary<string, BulletData> bulletData;
            private Dictionary<string, BulletSpawnerData> bulletSpawnerData;
            private Dictionary<string, WeaponData> weaponData;

            private Dictionary<string, T> LoadSingle<T>(IResMgr rm, string name) where T : Data
            {
                DataArray<T> dataArray = JsonUtility.FromJson<DataArray<T>>(rm.Get<TextAsset>("data", name).text);
                Dictionary<string, T> result = new Dictionary<string, T>();
                foreach (T i in dataArray.array)
                    result.Add(i.name, i);
                return result;
            }

            private T GetSingle<T>(Dictionary<string, T> data, string name) where T : Data
            {
                if (data.TryGetValue(name, out T result))
                    return result;
                else
                    return null;
            }
        }
    }
}