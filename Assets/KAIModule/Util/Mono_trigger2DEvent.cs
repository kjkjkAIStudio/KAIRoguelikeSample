using UnityEngine;

namespace KAIModule
{
    namespace Util
    {
        public class Mono_trigger2DEvent : MonoBehaviour
        {
            public event System.Action<Collider2D> OnTriggerEnter;
            public event System.Action<Collider2D> OnTriggerExit;

            public void OnTriggerEnter2D(Collider2D collision)
            {
                OnTriggerEnter?.Invoke(collision);
            }

            public void OnTriggerExit2D(Collider2D collision)
            {
                OnTriggerExit?.Invoke(collision);
            }
        }
    }
}