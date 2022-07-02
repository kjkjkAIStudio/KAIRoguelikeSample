namespace KAIModule
{
    namespace Helper
    {
        public interface IPoolElement
        {
            void Destroy();
            void Get();
            void Recycle();
        }
    }
}