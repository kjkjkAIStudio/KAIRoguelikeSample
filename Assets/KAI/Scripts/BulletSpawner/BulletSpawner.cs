using System.Collections;
using UnityEngine;
using KAI.MoveObjN;

namespace KAI
{
    namespace BulletSpawnerN
    {
        public class BulletSpawner
        {
            public BulletSpawnerData Data { get; private set; }
            public bool IsPlayerBullet { get; private set; }

            public BulletSpawner(BulletSpawnerData data, Transform t, bool isPlayerBullet)
            {
                Data = data;
                this.t = t;
                IsPlayerBullet = isPlayerBullet;
            }

            public void Destroy()
            {
                if (coro != null)
                {
                    Core.Inst.Mono.StopCoroutine(coro);
                    coro = null;
                }
            }

            /// <summary>
            /// When IsPlayerBullet is true, this will shoot once.
            /// </summary>
            public void Shoot()
            {
                if (coro != null)
                    Core.Inst.Mono.StopCoroutine(coro);
                coro = Coro();
                Core.Inst.Mono.StartCoroutine(coro);
            }

            private Transform t;
            private IEnumerator coro;

            private IEnumerator Coro()
            {
                while (true)
                {
                    foreach (BulletSpawnerData.TimeLine i in Data.timeLine)
                    {
                        BulletData data = Core.Inst.DM.GetBullet(i.bullet);
                        if (i.aimPlayer)
                            t.rotation = Quaternion.LookRotation(Vector3.forward, Core.Inst.GM.Player.H.transform.position - t.position);
                        else
                            t.localRotation = Quaternion.identity;
                        t.Rotate(Vector3.forward, i.rot);
                        for (int j = 0; j < i.count; ++j)
                        {
                            if (i.rndRot)
                                t.Rotate(Vector3.forward, Random.Range(i.rndRotMin, i.rndRotMax));
                            Bullet bullet = Core.Inst.GM.BulletPool.Get();
                            Core.Inst.GM.BulletDict.Add(bullet.Collider, bullet);
                            bullet.SetData(data, IsPlayerBullet);
                            bullet.TP(t.position);
                            bullet.H.transform.rotation = t.rotation;
                            bullet.OnQueryRecycle += (inst) => { Core.Inst.GM.BulletDict.Remove(inst.Collider); Core.Inst.GM.BulletPool.Recycle(inst); };
                            t.Rotate(Vector3.forward, i.deltaRot);
                        }
                        yield return new WaitForSeconds(i.time);
                    }
                    if (IsPlayerBullet)
                        break;
                }
            }
        }
    }
}