using System.Collections.Generic;

public class EditorBundleCollector : IBundleCollector
{
    public void Create(string name, Dictionary<string, IBundle> bundles)
    {
        Dictionary<string, HashSet<string>> info = new Dictionary<string, HashSet<string>>();
        BundleBuildRule.Instance.Collection(info);
        foreach (var kv in info)
        {
            EditorBundle bundle = new EditorBundle(kv.Key, kv.Value);
            bundles.Add(kv.Key, bundle);
        }
    }
}
