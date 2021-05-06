using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class PackageBundle : IBundle
{
    private readonly int nameHash;
    private readonly string path;
    private AssetBundle bundle;
    private AssetLoadRequest loadRequest;
    private readonly List<IBundle> depends = new List<IBundle>();

    public bool LoadFinish => bundle != null;
    public int NameHash => nameHash;
    public string Path => path;

    public PackageBundle(string name, string path)
    {
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
            }
            yield return new WaitUntil(() => !depends.Exists(it => !it.LoadFinish));
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
