namespace ECS.Core
{
    public interface IComponent
    {
        //重置成员变量为初始值，防止重用时逻辑错误
        void Reset();
    }

}

