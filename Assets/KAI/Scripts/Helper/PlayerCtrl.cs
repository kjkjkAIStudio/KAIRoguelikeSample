using UnityEngine;
using KAI.MoveObjN;

namespace KAI
{
    namespace Helper
    {
        public class PlayerCtrl
        {
            public bool CanCtrl { get; set; }
            public Player Player { get; set; }

            public PlayerCtrl()
            {
                
            }

            public void Update()
            {
                if (Player != null)
                {
                    if (CanCtrl)
                    {
                        Player.Dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                        Player.WeaponDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Player.H.transform.position;

                        if (Input.GetMouseButton(0))
                            Player.Fire();
                        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
                        {
                            if (Player.WeaponEquipIndex + 1 >= Player.MaxWeaponSlot)
                                Player.EquipWeapon(0);
                            else
                                Player.EquipWeapon(Player.WeaponEquipIndex + 1);
                        }
                        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
                        {
                            if (Player.WeaponEquipIndex - 1 < 0)
                                Player.EquipWeapon(Player.MaxWeaponSlot - 1);
                            else
                                Player.EquipWeapon(Player.WeaponEquipIndex - 1);
                        }
                    }
                    else
                    {
                        Player.Dir = Vector2.zero;
                    }

                    Core.Inst.Mono.ui_state.slider_hp.value = (float)Player.HP / Player.Data.maxHP;
                }
            }
        }
    }
}