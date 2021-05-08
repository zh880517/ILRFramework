using System.Collections.Generic;

public class AssetBundleManager
{
    private Dictionary<string, IBundle> bundles = new Dictionary<string, IBundle>();

    public void Init(string name)
    {
        IBundleCollector bundleCollector = null;
#if UNITY_EDITOR
        if (UnityEditor.EditorPrefs.GetBool("_useAssetBundle_", false))
        {
            bundleCollector = new EditorBundleCollector();
        }
#endif
        if (bundleCollector == null)
        {
            bundleCollector = new PackageBundleCollector();
        }
        bundleCollector.Create(name, bundles);
    }

    public void Destroy()
    {
        foreach (var kv in bundles)
        {
            kv.Value.UnLoad();
        }
        AssetLoader.StopAll();
    }

    public void LoadAsset<T>(string bundle, string asset, int key, System.Action<T, int, string> func) where T : UnityEngine.Object
    {
        if (bundles.TryGetValue(bundle, out IBundle assetBundle))
        {
            AssetLoader.Instance.StartCoroutine(assetBundle.LoadAsset(asset, key, func));
        }
    }

    public AssetLoadRequest<T> LoadAsset<T>(string bundle, string asset) where T : UnityEngine.Object
    {
        if (bundles.TryGetValue(bundle, out IBundle assetBundle))
        {
            AssetLoadRequest<T> request = new AssetLoadRequest<T> { BundleName = bundle, AssetName = asset };
            assetBundle.HandleRequest(request);
            return request;
        }
        return AssetLoadRequest<T>.Empty;
    }
}
