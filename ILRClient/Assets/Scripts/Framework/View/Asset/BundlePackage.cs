using System.IO;
using System.Collections.Generic;
//除了none外，其余的都会单独一个目录
public enum BundleGroupType
{
    None,
    Scene,
    Effect,
    Model,
    UI,
    Atals,
}

public class BundlePackage
{
    public BundleGroupType Group;
    public string FolderPath;
    //文件后缀名，使用|分割
    public string Pattern;
    //打包文件夹
    public bool PackFolder;
    //包含子目录
    public bool IncludeChildren;

    public void Collection(Dictionary<string, HashSet<string>> bundles)
    {
        string[] patterns = Pattern.Split(new char[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);
        if (!PackFolder)
        {
            foreach (var pattern in patterns)
            {
                var files = Directory.GetFiles(FolderPath, pattern, IncludeChildren ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    string bundleName = ToBundleName(fileName);
                    if (!bundles.TryGetValue(bundleName, out var list))
                    {
                        list = new HashSet<string>();
                        bundles.Add(bundleName, list);
                    }
                    list.Add(NormalizePath(file));
                }
            }
        }
        else
        {
            List<string> dirs = new List<string> { FolderPath };
            if (IncludeChildren)
            {
                dirs.AddRange(Directory.GetDirectories(FolderPath, "*", SearchOption.AllDirectories));
            }
            foreach (var dir in dirs)
            {
                string bundleName = ToBundleName(Path.GetDirectoryName(dir));
                bundles.TryGetValue(bundleName, out var list);
                foreach (var pattern in patterns)
                {
                    var files = Directory.GetFiles(dir, pattern);
                    if (files.Length > 0)
                    {
                        if (list != null)
                        {
                            list = new HashSet<string>();
                            bundles.Add(bundleName, list);
                        }
                        foreach (var file in files)
                        {
                            list.Add(NormalizePath(file));
                        }
                    }
                }
            }
        }
    }

    public string ToBundleName(string name)
    {
        if (Group == BundleGroupType.None)
        {
            return name.ToLower();
        }
        return string.Format("{0}/{1}", Group, name).ToLower();
    }

    public static string NormalizePath(string path)
    {
        path = path.Replace('\\', '/');
        return path;
    }
}
