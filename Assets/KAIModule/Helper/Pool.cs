using System.Collections.Generic;

namespace KAIModule
{
    namespace Helper
    {
        public class Pool<T> where T : IPoolElement
        {
            public Pool(System.Func<T> genFunc)
            {
                this.genFunc = genFunc;
                element = new Stack<T>();
            }

            public void PreGenerate(int count)
            {
                if (element.Count < count)
                {
                    int addCount = count - element.Count;
                    for (int i = 0; i < addCount; ++i)
                        element.Push(genFunc());
                }
                else if (element.Count > count)
                {
                    int decCount = element.Count - count;
                    for (int i = 0; i < decCount; ++i)
                    {
                        element.Pop().Destroy();
                    }
                }
            }

            public void Destroy()
            {
                foreach (T i in element)
                    i.Destroy();
                element.Clear();
            }

            public T Get()
            {
                T result;
                if (!element.TryPop(out result))
                {
                    result = genFunc();
                }
                result.Get();
                return result;
            }

            public void Recycle(T element)
            {
                element.Recycle();
                this.element.Push(element);
            }

            private System.Func<T> genFunc;
            private Stack<T> element;
        }
    }
}