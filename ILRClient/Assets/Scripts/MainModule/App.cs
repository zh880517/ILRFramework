using UnityEngine;

public class App : MonoBehaviour
{
    public static App Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
#if UNITY_EDITOR
        Hotfix.InitByPath("BuildOutput/", "Hotfix");
#else
        Hotfix.Init(Resources.Load<TextAsset>("Data/HotFix.dll").bytes, null);
        
#endif
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
