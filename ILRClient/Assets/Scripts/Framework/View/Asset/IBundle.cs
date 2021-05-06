using System.Collections;
using UnityEngine;

public interface IBundle
{
    int NameHash { get; }
    string Path { get; }
    void AddDepend(IBundle bundle);
    IEnumerator Load();
    bool LoadFinish { get; }
    IEnumerator LoadAsset<T>(string asset, System.Action<T, string> func) where T : Object;

    void UnLoad();
}
