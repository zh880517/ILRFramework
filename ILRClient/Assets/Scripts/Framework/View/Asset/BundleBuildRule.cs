using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BundleBuildRule
{
    static BundleBuildRule _instance;
    public static BundleBuildRule Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = ReadFromFile();
            }
            return _instance;
        }
    }

    public const string SaveFile = "ProjectSettings/BundleBuildConfig.json";
    public List<BundlePackage> Packages = new List<BundlePackage>();

    public void Collection(Dictionary<string, HashSet<string>> bundles)
    {
        foreach (var pkg in Packages)
        {
            pkg.Collection(bundles);
        }
    }

    public void Save()
    {
        File.WriteAllText(SaveFile, JsonUtility.ToJson(this));
    }
    static BundleBuildRule ReadFromFile()
    {
        return JsonUtility.FromJson<BundleBuildRule>(File.ReadAllText(SaveFile));
    }
}
