public interface IGameScene
{
    void OnCreate();
    void OnLoad();
    void OnTick(long passMS);
    void OnExit();
}
