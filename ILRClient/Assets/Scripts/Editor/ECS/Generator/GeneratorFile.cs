using System.Collections.Generic;
using System.IO;

public class GeneratorFolder
{
    private Dictionary<string, string> FileContents = new Dictionary<string, string>();
    public string DirectoryPath { get; private set; }
    private bool DeleteOldFile;

    public GeneratorFolder(string path, bool deleteOldFile)
    {
        DirectoryPath = path;
        if (!path.EndsWith("/"))
            DirectoryPath += "/";
        DeleteOldFile = deleteOldFile;
    }

    public void AddFile(string name, string content)
    {
        FileContents[name] = content;
    }

    public void WriteFile(string externName)
    {
        var dirInfo = new DirectoryInfo(DirectoryPath);
        if (dirInfo.Exists)
        {
            if (DeleteOldFile)
            {
                var files = dirInfo.GetFiles(string.Format("*.{0}", externName));
                foreach (var file in files)
                {
                    if (!FileContents.ContainsKey(file.Name))
                    {
                        file.Delete();
                    }
                }
            }
        }
        else
        {
            dirInfo.Create();
        }
        foreach (var kv in FileContents)
        {
            string filePath = string.Format("{0}{1}.{2}", DirectoryPath, kv.Key, externName);
            if (File.Exists(filePath))
            {
                if (File.ReadAllText(filePath) == kv.Value)
                {
                    continue;
                }
            }
            File.WriteAllText(filePath, kv.Value);
        }
    }

}
