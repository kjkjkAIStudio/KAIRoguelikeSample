using System.Collections;
using UnityEngine;
using KAIModule.Helper;

namespace KAI
{
    namespace MoveObjN
    {
        public class Bullet : MoveObj, IPoolElement
        {
            public BulletData Data { get; private set; }
            public bool IsPlayerBullet { get; private set; }
            public new Collider2D Collider { get { return base.Collider; } }

            public event System.Action<Bullet> OnQueryRecycle;

            public Bullet() : base()
            {
                BindH(Object.Instantiate(Core.Inst.Mono.prefab_bullet));
                MonoTrigger.OnTriggerEnter += OnTriggerEnter;
            }

            public override void FixedUpdate()
            {
                H.transform.Rotate(Vector3.forward, rotSpeed * Time.fixedDeltaTime);
                H.transform.localScale += (Vector3)sclSpeed * Time.fixedDeltaTime;
                Speed = speed;
                Dir = H.transform.up;
                speed += acc * Time.fixedDeltaTime;
                base.FixedUpdate();
            }

            public void Get()
            {
                H.transform.rotation = Quaternion.identity;
                H.transform.localScale = Vector3.one;
                SetHitBox(true);
                H.SetActive(true);
            }

            public void Recycle()
            {
                MonoTrigger.StopAllCoroutines();
                H.SetActive(false);
            }

            public void SetData(BulletData data, bool isPlayerBullet)
            {
                Data = data;
                IsPlayerBullet = isPlayerBullet;
                MonoTrigger.StartCoroutine(Coro_timeLine());
                MonoTrigger.StartCoroutine(Coro_frame());
            }

            public void HitImmediate()
            {
                //Make sure not to recycle twice.
                if (H.activeSelf)
                {
                    OnQueryRecycle?.Invoke(this);
                    OnQueryRecycle = null;
                }
            }

            public void Hit()
            {
                SetHitBox(false);
                speed = 0.0f;
                acc = 0.0f;
                rotSpeed = 0.0f;
                sclSpeed = Vector2.zero;
                MonoTrigger.StopAllCoroutines();
                MonoTrigger.StartCoroutine(Coro_frameHit());
            }

            private float speed, acc;
            private float rotSpeed;
            private Vector2 sclSpeed;

            protected void OnTriggerEnter(Collider2D target)
            {
                if (target.tag == "env")
                    HitImmediate();
            }

            private IEnumerator Coro_timeLine()
            {
                foreach (BulletData.TimeLine i in Data.timeLine)
                {
                    speed = i.rndSpeed ? i.speed + Random.Range(i.rndSpeedMin, i.rndSpeedMax) : i.speed;
                    acc = i.rndAcc ? i.acc + Random.Range(i.rndAccMin, i.rndAccMax) : i.acc;
                    rotSpeed = i.rndRotSpeed ? i.rotSpeed + Random.Range(i.rndRotSpeedMin, i.rndRotSpeedMax) : i.rotSpeed;
                    sclSpeed = i.rndSclSpeed ? i.sclSpeed + new Vector2(Random.Range(i.rndSclSpeedMin.x, i.rndSclSpeedMax.x),
                        Random.Range(i.rndSclSpeedMin.y, i.rndSclSpeedMax.y))
                        : i.sclSpeed;
                    if (i.aimPlayer)
                        H.transform.rotation = Quaternion.LookRotation(Vector3.forward, Core.Inst.GM.Player.H.transform.position - H.transform.position);
                    yield return new WaitForSeconds(i.time);
                }
                HitImmediate();
            }

            private IEnumerator Coro_frame()
            {
                int cFrame = -1;
                float delay = 1.0f / Data.frameRate;
                while (true)
                {
                    ++cFrame;
                    if (cFrame >= Data.resFrameCount)
                        cFrame = 0;
                    Img.sprite = Core.Inst.RM.Get<Sprite>("bullet", Data.resName, cFrame);
                    yield return new WaitForSeconds(delay);
                }
            }

            private IEnumerator Coro_frameHit()
            {
                int cFrame = -1;
                float delay = 1.0f / Data.frameRate;
                while (true)
                {
                    ++cFrame;
                    if (cFrame >= Data.resHitFrameCount)
                    {
                        HitImmediate();
                        yield break;
                    }
                    Img.sprite = Core.Inst.RM.Get<Sprite>("bullet", Data.resHitName, cFrame);
                    yield return new WaitForSeconds(delay);
                }
            }
        }
    }
}