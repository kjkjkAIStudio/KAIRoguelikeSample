using UnityEngine;
using KAI.WeaponN;

namespace KAI
{
    namespace MoveObjN
    {
        public class Player : LiveObj
        {
            public PlayerData Data { get; private set; }
            public int MaxWeaponSlot { get { return 3; } }
            public int WeaponEquipIndex { get; private set; }
            public Vector2 WeaponDir { get; set; }

            public Player(PlayerData data) : base()
            {
                Data = data;
                Speed = data.speed;
                HP = data.maxHP;
                WeaponEquipIndex = -1;
                weapon = new Weapon[MaxWeaponSlot];

                Core.Inst.RM.Open(data.name);
                BindH(Object.Instantiate(Core.Inst.RM.Get<GameObject>(data.name, "char"), Core.Inst.Mono.transform));
                Core.Inst.RM.Close(data.name);
                MonoTrigger.OnTriggerEnter += OnTriggerEnter;
                t_weaponSlot = new GameObject().transform;
                t_weaponSlot.SetParent(H.transform);

                for (int i = 0; i < weapon.Length; ++i)
                    if (!string.IsNullOrEmpty(data.startWeapon[i]))
                        ChangeWeapon(i, Core.Inst.DM.GetWeapon(data.startWeapon[i]));
                EquipWeapon(0);
            }

            public override void Update()
            {
                base.Update();
                foreach (Weapon i in weapon)
                    if (i != null)
                        i.Update();
                Vector3 weaponDir = WeaponDir.normalized;
                t_weaponSlot.localPosition = weaponDir * 1.0f;
                t_weaponSlot.rotation = Quaternion.LookRotation(Vector3.forward, weaponDir);
                t_weaponSlot.localScale = new Vector3(weaponDir.x < 0.0f ? -1.0f : 1.0f, 1.0f, 1.0f);
            }

            public void ChangeWeapon(int index, WeaponData data)
            {
                if (weapon[index] != null)
                    weapon[index].Destroy();
                weapon[index] = new Weapon(data, t_weaponSlot);
            }

            public void EquipWeapon(int index)
            {
                if (index == WeaponEquipIndex)
                    return;
                if (WeaponEquipIndex != -1)
                    weapon[WeaponEquipIndex].SetVisible(false);
                WeaponEquipIndex = index;
                weapon[index].SetVisible(true);
            }

            public void Fire()
            {
                weapon[WeaponEquipIndex].Fire();
            }

            private Transform t_weaponSlot;
            private Weapon[] weapon;

            private void OnTriggerEnter(Collider2D target)
            {
                if (target.gameObject.tag == "bullet")
                {
                    if (Core.Inst.GM.BulletDict.TryGetValue(target, out Bullet bullet) && !bullet.IsPlayerBullet)
                    {
                        bullet.Hit();
                        Hurt(bullet.Data.damage);
                    }
                }
                else if (target.gameObject.name == "nextStage")
                {
                    Core.Inst.GM.NextStage();
                }
            }
        }
    }
}