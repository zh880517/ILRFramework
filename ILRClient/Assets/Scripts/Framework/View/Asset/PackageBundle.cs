using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class PackageBundle : IBundle
{
    private readonly int nameHash;
    private readonly string name;
    private readonly string path;
    private AssetBundle bundle;
    private AssetBundleLoadRequest loadRequest;
    private readonly List<IBundle> depends = new List<IBundle>();

    public string Name => name;
    public bool LoadFinish => loadRequest == null;

    public PackageBundle(string name, string path)
    {
        this.name = name;
        nameHash = Animator.StringToHash(name);
        this.path = path;
    }

    public void AddDepend(IBundle bundle)
    {
        depends.Add(bundle);
    }

    public IEnumerator Load()
    {
        if (bundle == null)
        {
            if (loadRequest == null)
            {
                foreach (var bundle in depends)
                {
                    bundle.Load();
                }
                loadRequest = AssetLoader.Instance.Load(nameHash, path);
            }

            if(loadRequest != null)
            {
                yield return loadRequest;
                bundle = loadRequest.bundle;
                loadRequest = null;
            }

            foreach (var bundle in depends)
            {
                if (bundle is PackageBundle pakBundle && pakBundle.loadRequest != null)
                {
                    yield return pakBundle.loadRequest;
                }
            }
        }
    }

    public IEnumerator LoadAsset<T>(string asset, int key, Action<T, int, string> func) where T : UnityEngine.Object
    {
        if (bundle == null)
            yield return Load();
        if (bundle != null)
        {
            var req = bundle.LoadAssetAsync<T>(asset);
            yield return req;
            func?.Invoke(req.asset as T, key, asset);
        }
        else
        {
            func?.Invoke(null, key, asset);
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

    public void HandleRequest<T>(AssetLoadRequest<T> request) where T : UnityEngine.Object
    {
        AssetLoader.Instance.AddTask(() => { return DoLoadRequest(request); });
    }

    private IEnumerator DoLoadRequest<T>(AssetLoadRequest<T> request) where T : UnityEngine.Object
    {
        if (bundle == null)
            yield return Load();
        if (bundle != null)
        {
            var req = bundle.LoadAssetAsync<T>(request.AssetName);
            yield return req;
            request.Asset = req.asset as T;
        }
        request.LoadFinsh = true;

    }
}
