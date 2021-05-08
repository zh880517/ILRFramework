using System.Collections;
using UnityEngine;

public interface IBundle
{
    string Name { get; }
    void AddDepend(IBundle bundle);
    IEnumerator Load();
    bool LoadFinish { get; }
    IEnumerator LoadAsset<T>(string asset, int key, System.Action<T, int, string> func) where T : Object;
    void HandleRequest<T>(AssetLoadRequest<T> request) where T : Object;
    void UnLoad();
}
