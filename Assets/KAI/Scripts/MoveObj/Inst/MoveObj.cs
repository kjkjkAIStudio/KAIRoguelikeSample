using UnityEngine;
using KAIModule.Util;

namespace KAI
{
    namespace MoveObjN
    {
        public class MoveObj
        {
            public GameObject H { get; private set; }

            public float Speed { get; set; }
            public Vector2 Dir { get; set; }

            public MoveObj()
            {

            }

            public virtual void Update()
            {

            }

            public virtual void FixedUpdate()
            {
                Rigidbody.velocity = Dir.normalized * Speed;
            }

            public virtual void Destroy()
            {
                Object.Destroy(H);
            }

            public void TP(Vector2 pos)
            {
                H.transform.position = pos;
            }

            public void SetHitBox(bool v)
            {
                Collider.enabled = v;
            }

            public void SetVisible(bool v)
            {
                Img.enabled = v;
            }

            protected Rigidbody2D Rigidbody { get; private set; }
            protected Collider2D Collider { get; private set; }
            protected Mono_trigger2DEvent MonoTrigger { get; private set; }
            protected SpriteRenderer Img { get; private set; }

            protected virtual void BindH(GameObject h)
            {
                H = h;
                Rigidbody = h.GetComponent<Rigidbody2D>();
                Collider = h.GetComponent<Collider2D>();
                MonoTrigger = h.AddComponent<Mono_trigger2DEvent>();
                Img = h.transform.Find("img").GetComponent<SpriteRenderer>();
            }
        }
    }
}