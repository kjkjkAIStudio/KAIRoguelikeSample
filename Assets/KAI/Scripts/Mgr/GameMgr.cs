using System.Collections.Generic;
using UnityEngine;
using KAI.MoveObjN;
using KAI.Helper;
using KAIModule.Helper;

namespace KAI
{
    namespace Mgr
    {
        public class GameMgr
        {
            public Player Player { get; private set; }
            public HashSet<Enemy> EnemyDict { get; private set; }
            public Pool<Bullet> BulletPool { get; private set; }
            public Dictionary<Collider2D, Bullet> BulletDict { get; private set; }

            public PlayerCtrl PlayerCtrl { get; private set; }
            public EnemySpawner EnemySpawner { get; private set; }

            public GameMgr()
            {
                cStage = -1;
                EnemyDict = new HashSet<Enemy>();
                BulletPool = new Pool<Bullet>(() =>
                {
                    Bullet bullet = new Bullet();
                    bullet.H.transform.SetParent(Core.Inst.Mono.transform);
                    bullet.H.SetActive(false);
                    return bullet;
                });
                BulletPool.PreGenerate(100);
                BulletDict = new Dictionary<Collider2D, Bullet>();
                PlayerCtrl = new PlayerCtrl();
                EnemySpawner = new EnemySpawner();
                EnemySpawner.MaxEnemyCount = 10;
            }

            public void Update()
            {
                PlayerCtrl.Update();
                EnemySpawner.Update();
                if (Player != null)
                    Player.Update();
            }

            public void FixedUpdate()
            {
                if (Player != null)
                    Player.FixedUpdate();
                foreach (Enemy i in EnemyDict)
                    i.FixedUpdate();
                foreach (Bullet i in BulletDict.Values)
                    i.FixedUpdate();
            }

            public void StartGame()
            {
                cStage = -1;

                Core.Inst.RM.Open("enemy");
                Core.Inst.RM.Open("bullet");
                Core.Inst.RM.Open("weapon");

                Player = new Player(Core.Inst.DM.GetPlayer("char_0"));
                Player.SetHitBox(false);
                Player.SetVisible(false);
                Player.OnDie += (p) =>
                {
                    Core.Inst.ShowDlg("You died. Game will be restarted.", Core.Inst.EndGame);
                };
                PlayerCtrl.Player = Player;
                Core.Inst.CamM.T_target = Player.H.transform;
                Core.Inst.Mono.ui_state.h.SetActive(true);

                NextStage();
                Core.Inst.ShowDlg("Tutorial\nWASD or arrow key to move.\nLMB to fire.\nMouse scrollWheel to change weapon.");
            }

            public void EndGame()
            {
                Core.Inst.Mono.ui_state.h.SetActive(false);
                Core.Inst.CamM.T_target = null;
                EnemySpawner.IsSpawn = false;
                PlayerCtrl.CanCtrl = false;

                List<Bullet> cpy = new List<Bullet>(BulletDict.Values);
                foreach (Bullet i in cpy)
                    i.HitImmediate();
                BulletPool.Destroy();
                foreach (Enemy i in EnemyDict)
                    i.Destroy();
                EnemyDict.Clear();
                Player.Destroy();

                Core.Inst.RM.Close("weapon");
                Core.Inst.RM.Close("bullet");
                Core.Inst.RM.Close("enemy");
            }

            public void NextStage()
            {
                Player.SetHitBox(false);
                foreach (Enemy i in EnemyDict)
                    i.Destroy();
                EnemyDict.Clear();
                List<Bullet> cpy = new List<Bullet>(BulletDict.Values);
                foreach (Bullet i in cpy)
                    i.HitImmediate();
                PlayerCtrl.CanCtrl = false;
                EnemySpawner.IsSpawn = false;

                ++cStage;
                Core.Inst.StageM.ToStage(cStage, (mono) =>
                {
                    if (mono != null)
                    {
                        Player.TP(mono.t_startPos.position);
                        Player.SetHitBox(true);
                        Player.SetVisible(true);
                        PlayerCtrl.CanCtrl = true;
                        EnemySpawner.SetData(mono.t_enemySpawner);
                        EnemySpawner.IsSpawn = true;
                        Core.Inst.CamM.TP(mono.t_startPos.position);
                        Core.Inst.Mono.ui_state.txt_stage.text = string.Format("Stage: {0}", cStage + 1);
                    }
                    else
                    {
                        Core.Inst.ShowDlg("This is the final stage. Game will be restarted.", Core.Inst.EndGame);
                    }
                });
            }

            private int cStage;
        }
    }
}