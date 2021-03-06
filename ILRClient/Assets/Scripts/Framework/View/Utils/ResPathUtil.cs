using System.IO;
using UnityEngine;

public static class ResPathUtil
{
    //资源WWW加载目录
    private static string streamAssetUrl;
    public static string StreamAssetUrl
    {
        get
        {
            if (streamAssetUrl == null)
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        streamAssetUrl = "jar:file://" + Application.dataPath + "!/assets/";
                        break;
                    case RuntimePlatform.IPhonePlayer:
                        streamAssetUrl = "file:///" + Application.dataPath + "/Raw/";
                        break;
                    default:
                        streamAssetUrl = "file://" + Application.streamingAssetsPath + "/";
                        break;
                }
            }
            return streamAssetUrl;
        }
    }

    //资源直接读取路径
    private static string streamAssetPath;
    public static string StreamAssetPath
    {
        get
        {
            if (streamAssetPath == null)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    streamAssetPath = Application.dataPath + "!assets/";
                }
                else
                {
                    streamAssetPath = Application.streamingAssetsPath + "/";
                }
            }
            return streamAssetPath;
        }
    }

    //热更新文件存放目录
    private static string hotPatchPath;
    public static string HotPatchPath
    {
        get
        {
            if (hotPatchPath == null)
            {
#if UNITY_EDITOR
                hotPatchPath = Path.GetFullPath("../Build/").Replace("\\", "/");
#else
                
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    hotPatchPath = Application.dataPath + "/patch/";
                }
                else
                {
                    hotPatchPath = Application.persistentDataPath + "/patch/";
                }
#endif
            }
            return hotPatchPath;
        }
    }

    //热更新文件Url路径
    private static string hotPatchUrl;
    public static string HotPatchUrl
    {
        get
        {
            if (hotPatchUrl == null)
            {
                hotPatchUrl = "file:///" + HotPatchPath;
            }
            return hotPatchUrl;
        }
    }

    public static string GetStreamAssetFileUrl(string name, bool checkPatch = true)
    {
        if (checkPatch && File.Exists(HotPatchPath + name))
            return HotPatchUrl + name;
        return StreamAssetUrl + name;
    }

    public static string GetStreamAssetFilePath(string name, bool checkPatch = true)
    {
        if (checkPatch)
        {
            string hotPatchPath = HotPatchPath + name;
            if (File.Exists(hotPatchPath))
                return hotPatchPath;
        }
        return StreamAssetPath + name;
    }
    
    public static string MakeFilePath(string file)
    {
        return HotPatchPath + file;
    }

}
