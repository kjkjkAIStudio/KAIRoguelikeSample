using System.Collections;
using UnityEngine;
using KAI.BulletSpawnerN;

namespace KAI
{
    namespace MoveObjN
    {
        public class Enemy : LiveObj
        {
            public EnemyData Data { get; private set; }
            public BulletSpawner BS { get; private set; }

            public event System.Action<Enemy> OnQueryDestroy;

            public Enemy(EnemyData data) : base()
            {
                Data = data;
                Speed = data.speed;
                HP = data.maxHP;

                BindH(Object.Instantiate(Core.Inst.RM.Get<GameObject>("enemy", data.name)));
                MonoTrigger.OnTriggerEnter += OnTriggerEnter;
                MonoTrigger.StartCoroutine(Coro());
                Transform t_bs = new GameObject().transform;
                t_bs.SetParent(H.transform);
                BS = new BulletSpawner(Core.Inst.DM.GetBS(data.bulletSpawner), t_bs, false);
            }

            public override void Destroy()
            {
                base.Destroy();
                BS.Destroy();
            }

            public override void Die()
            {
                base.Die();
                BS.Destroy();
                MonoTrigger.StartCoroutine(Coro_die());
            }

            private const float MinChangeMindDelay = 0.5f, MaxChangeMindDelay = 3.0f;

            private void OnTriggerEnter(Collider2D target)
            {
                if (target.gameObject.tag == "bullet")
                {
                    if (Core.Inst.GM.BulletDict.TryGetValue(target, out Bullet bullet) && bullet.IsPlayerBullet)
                    {
                        bullet.Hit();
                        Hurt(bullet.Data.damage);
                    }
                }
            }

            private IEnumerator Coro()
            {
                while (!IsDie)
                {
                    Dir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                    float delay = Random.Range(MinChangeMindDelay, MaxChangeMindDelay);
                    yield return new WaitForSeconds(delay);
                }
            }

            private IEnumerator Coro_die()
            {
                yield return new WaitForSeconds(1.0f);
                OnQueryDestroy?.Invoke(this);
            }
        }
    }
}