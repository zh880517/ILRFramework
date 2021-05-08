using System.Collections.Generic;
using UnityEngine;

public class PackageBundleCollector : IBundleCollector
{
    public void Create(string name, Dictionary<string, IBundle> bundles)
    {
        string manifestFileName = "StreamingAssets";
        System.Func<string, string> getPath = (string key)=> { return name; };
        if (!string.IsNullOrEmpty(name))
        {
            manifestFileName = $"{name}/{name}";
            getPath = (string key) => { return string.Format("{0}/{1}", name, key); };
        }
        string manifestFile = ResPathUtil.GetStreamAssetFilePath(manifestFileName);
        var ab = AssetBundle.LoadFromFile(manifestFile);
        if (ab == null)
        {
            Debug.LogErrorFormat("资源初始化失败 => {0}加载失败", manifestFileName);
            return;
        }
        var manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        var allABName = manifest.GetAllAssetBundles();
        Dictionary<string, IBundle> cache = new Dictionary<string, IBundle>();
        foreach (var abName in allABName)
        {
            string keyName = name.Substring(0, abName.LastIndexOf('.'));
            PackageBundle bundle = new PackageBundle(keyName, getPath(abName));
            bundles.Add(keyName, bundle);
            cache.Add(abName, bundle);
        }
        foreach (var abName in allABName)
        {
            var depens = manifest.GetAllDependencies(abName);
            IBundle bundle = cache[abName];
            foreach (var dep in depens)
            {
                bundle.AddDepend(cache[dep]);
            }
        }

        ab.Unload(true);
    }
}
