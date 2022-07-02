using UnityEngine;
using KAI.BulletSpawnerN;

namespace KAI
{
    namespace WeaponN
    {
        public class Weapon
        {
            public WeaponData Data { get; private set; }

            public Weapon(WeaponData data, Transform t_weaponSlot)
            {
                Data = data;
                h_weapon = Object.Instantiate(Core.Inst.RM.Get<GameObject>("weapon", data.name), t_weaponSlot);
                SetVisible(false);
                bs = new BulletSpawner(Core.Inst.DM.GetBS(data.bulletSpawner), h_weapon.transform.Find("bulletSpawner"), true);
            }

            public void Destroy()
            {
                bs.Destroy();
                Object.Destroy(h_weapon);
            }

            public void SetVisible(bool v)
            {
                h_weapon.SetActive(v);
            }

            public void Update()
            {
                cd -= Time.deltaTime;
                if (cd < 0.0f)
                    cd = 0.0f;
            }

            public void Fire()
            {
                if (cd <= 0.0f)
                {
                    cd = Data.cd;
                    bs.Shoot();
                }
            }

            private float cd;
            private GameObject h_weapon;
            private BulletSpawner bs;
        }
    }
}