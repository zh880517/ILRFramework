using System;
using System.Collections;
using UnityEngine;

public class PackageBundle : IBundle
{
    private AssetBundle bundle;
    private bool isLoading;
    public bool LoadFinish => !isLoading || bundle != null;
    public IEnumerator Load()
    {
        if (bundle == null)
        {
            if (isLoading)
            {
                yield return new WaitUntil(() => LoadFinish);
            }
            else
            {
                isLoading = true;
                //todo：添加到加载队列
                isLoading = false;
            }
        }
    }

    public IEnumerator LoadAsset<T>(string asset, Action<T, string> func) where T : UnityEngine.Object
    {
        if (bundle == null)
            yield return Load();
        if (bundle != null)
        {
            var req = bundle.LoadAssetAsync<T>(asset);
            yield return req;
            func?.Invoke(req.asset as T, asset);
        }
        else
        {
            func?.Invoke(null, asset);
        }
    }

    public void UnLoad()
    {
        if (bundle != null)
        {
            bundle.Unload(true);
            bundle = null;
        }
    }
}
