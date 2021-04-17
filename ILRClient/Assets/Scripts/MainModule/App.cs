using UnityEngine;

public class App : MonoBehaviour
{
    public static App Instance { get; private set; }
    private void Awake()
    {
        Instance = this;

        Hotfix.InitByPath("BuildOutput/", "Hotfix");
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
