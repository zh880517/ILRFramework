namespace ECS.Core
{
    public interface IInitializeSystem : ISystem
    {
        void OnInitialize();
    }
}