namespace ECS.Core
{
    /// <summary>
    /// 唯一属性
    /// 标记的Component在整个Context只能存在一个，向一个新的Entity添加的时候回自动从之前的Entity移除
    /// </summary>
    public class UniqueAttribute : ComponentAttribute
    {
    
    }
}