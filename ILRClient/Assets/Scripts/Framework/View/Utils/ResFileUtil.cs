using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class ResFileUtil
{
    public static string ReadString(string filePath, bool onlyHotPatch = false)
    {
        string hotPatchPath = ResPathUtil.HotPatchPath + filePath;
        if (File.Exists(hotPatchPath))
            return File.ReadAllText(hotPatchPath);

        if (onlyHotPatch)
            return null;

        int index = filePath.LastIndexOf('.');
        if (index > 0)
            filePath = filePath.Substring(0, index);
        var txtAsset = Resources.Load<TextAsset>(filePath);
        return txtAsset?.text;
    }


    public static byte[] ReadBytes(string filePath, bool onlyHotPatch = false)
    {
        string hotPatchPath = ResPathUtil.HotPatchPath + filePath;
        if (File.Exists(hotPatchPath))
            return File.ReadAllBytes(hotPatchPath);

        if (onlyHotPatch)
            return null;

        int index = filePath.LastIndexOf('.');
        if (index > 0)
            filePath = filePath.Substring(0, index);
        var txtAsset = Resources.Load<TextAsset>(filePath);
        return txtAsset?.bytes;
    }

    public static string ReadStreamAssetString(string filePath)
    {
        string hotPatchPath = ResPathUtil.HotPatchPath + filePath;
        if (File.Exists(hotPatchPath))
            return File.ReadAllText(hotPatchPath);
#if UNITY_EDITOR || !UNITY_ANDROID
        string fullPath = ResPathUtil.StreamAssetPath + filePath;
        if (File.Exists(fullPath))
            return File.ReadAllText(fullPath);
#endif
        return null;
    }

    public static byte[] ReadStreamAssetBytes(string filePath)
    {
        string hotPatchPath = ResPathUtil.HotPatchPath + filePath;
        if (File.Exists(hotPatchPath))
            return File.ReadAllBytes(hotPatchPath);
#if UNITY_EDITOR || !UNITY_ANDROID
        string fullPath = ResPathUtil.StreamAssetPath + filePath;
        if (File.Exists(fullPath))
            return File.ReadAllBytes(fullPath);
#endif
        return null;
    }

    public static string FindHotPathFilePath(string filePath)
    {
        string hotPatchPath = ResPathUtil.HotPatchPath + filePath;
        if (File.Exists(hotPatchPath))
            return hotPatchPath;
        return null;
    }

    public static string WriteFile(string fileName, string txt)
    {
        string fullPath = ResPathUtil.HotPatchPath + fileName;
        CheckPath(fullPath);
        File.WriteAllText(fullPath, txt);
        return fullPath;
    }

    public static string WriteFile(string fileName, byte[] bytes)
    {
        string fullPath = ResPathUtil.HotPatchPath + fileName;
        CheckPath(fullPath);
        File.WriteAllBytes(fullPath, bytes);
        return fullPath;
    }

    public static void DeleteFile(string fileName)
    {
        string fullPath = ResPathUtil.HotPatchPath + fileName;
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    public static void CreateFolder(string path)
    {
        string fullPath = ResPathUtil.HotPatchPath + path;
        if (!Directory.Exists(fullPath))
            Directory.CreateDirectory(fullPath);
    }

    public static void DeleteFolder(string path)
    {
        string fullPath = ResPathUtil.HotPatchPath + path;
        if (Directory.Exists(fullPath))
            Directory.Delete(fullPath, true);
    }

    public static void CopyFolder(string src, string dest)
    {
        DirectoryInfo srcFolder = new DirectoryInfo(src);
        if (!srcFolder.Exists)
            return;
        if (!Directory.Exists(dest))
            Directory.CreateDirectory(dest);
        var files = srcFolder.GetFiles();
        foreach (var file in files)
        {
            file.CopyTo(Path.Combine(dest, file.Name), true);
        }
        var children = srcFolder.GetDirectories();
        foreach (var child in children)
        {
            CopyFolder(child.FullName, Path.Combine(dest, child.Name));
        }
    }

    public static void CheckPath(string path)
    {
        string dir = Path.GetDirectoryName(path);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }

    public static string Md5(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("md5file() fail, error:" + ex.Message);
            return null;
        }
    }

    public static string Md5ResFile(string file)
    {
        string fullPath = ResPathUtil.HotPatchPath + file;
        return Md5(file);
    }
}
