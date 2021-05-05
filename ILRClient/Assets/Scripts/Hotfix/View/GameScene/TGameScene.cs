public abstract class TGameScene<T> : IGameScene
{
    protected T Data;
    
    public TGameScene(T data)
    {
        Data = data;
    }

    public abstract void OnCreate();

    public abstract void OnExit();

    public abstract void OnLoad();

    public abstract void OnTick(long passMS);
}
