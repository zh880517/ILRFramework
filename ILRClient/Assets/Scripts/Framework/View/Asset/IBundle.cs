using System.Collections;
using UnityEngine;

public interface IBundle
{
    IEnumerator Load();

    IEnumerator LoadAsset<T>(string asset, System.Action<T, string> func) where T : Object;

    void UnLoad();
}
