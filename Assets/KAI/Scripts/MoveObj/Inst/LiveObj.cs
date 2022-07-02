using UnityEngine;

namespace KAI
{
    namespace MoveObjN
    {
        public class LiveObj : MoveObj
        {
            public int HP { get; set; }
            public bool IsDie { get; private set; }

            public event System.Action<LiveObj> OnDie;

            public LiveObj() : base()
            {

            }

            public override void FixedUpdate()
            {
                if (IsDie)
                    return;

                base.FixedUpdate();
                Ani.SetBool("move", Dir.magnitude > 0.0f);
                if (Dir.x < 0.0f)
                    Img.flipX = true;
                else if (Dir.x > 0.0f)
                    Img.flipX = false;
            }

            public void ModHP(int v)
            {
                HP += v;
                if (HP <= 0)
                    Die();
            }

            public void Hurt(int v)
            {
                ModHP(-v);
                Ani.SetTrigger("hurt");
            }

            public virtual void Die()
            {
                IsDie = true;
                SetHitBox(false);
                Rigidbody.velocity = Vector2.zero;
                Ani.SetBool("die", true);
                OnDie?.Invoke(this);
            }

            protected Animator Ani { get; private set; }

            protected override void BindH(GameObject h)
            {
                base.BindH(h);
                Ani = h.GetComponent<Animator>();
            }
        }
    }
}