
namespace ECS.Core
{
    public interface ICleanupSystem : ISystem
    {
        void OnCleanup();
    }
}