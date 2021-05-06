using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AssetLoader : MonoBehaviour
{
    private static AssetLoader _instance;
    public static AssetLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("_AssetLoadQueue_");
                _instance = go.AddComponent<AssetLoader>();
            }
            return _instance;
        }
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    private Dictionary<int, AssetLoadRequest> loadingRequests = new Dictionary<int, AssetLoadRequest>();
    private Queue<int> waits = new Queue<int>();
    const int QueueMaxCount = 40;
    private int loadCount = 0;

    uint version = 0;

    public AssetLoadRequest Load(int hasId,string path)
    {
        if (loadingRequests.TryGetValue(hasId, out var request))
        {
            return request;
        }
        request = new AssetLoadRequest
        {
            path = path
        };
        loadingRequests.Add(hasId, request);
        waits.Enqueue(hasId);
        if (QueueMaxCount <= 0 && loadCount < QueueMaxCount)
        {
            StartCoroutine(DoLoad());
        }
        return request;
    }

    private IEnumerator DoLoad()
    {
        while (waits.Count > 0)
        {
            int id = waits.Dequeue();
            if (loadingRequests.TryGetValue(id, out var request))
            {
                yield return LoadAssetBundle(id, request);
            }
        }
        --loadCount;
    }

    private IEnumerator LoadAssetBundle(int id, AssetLoadRequest request)
    {
#if UNITY_EDITOR || !UNITY_ANDROID
        yield return LoadNoneCache(id, request);
#else

#endif
        if (request.bundle != null)
        {
            string hotPatchPath = ResFileUtil.FindHotPathFilePath(request.path);
            if (hotPatchPath != null)
            {
                yield return LoadNoneCache(id, request);
            }
        }
    }

    private IEnumerator LoadByCache(int id, AssetLoadRequest request)
    {
        string url = ResPathUtil.GetStreamAssetFileUrl(request.path, false);
        UnityWebRequest download = UnityWebRequestAssetBundle.GetAssetBundle(url, version, 0);
        yield return download.SendWebRequest();
        if (download.result == UnityWebRequest.Result.Success)
        {
            request.bundle = DownloadHandlerAssetBundle.GetContent(download);
        }
        if (request.bundle == null)
        {
            Debug.LogError("AssetBundle 加载失败 => " + request.path);
        }
    }

    private IEnumerator LoadNoneCache(int id, AssetLoadRequest request)
    {
        string path = ResPathUtil.GetStreamAssetFilePath(request.path);
        var abcr = AssetBundle.LoadFromFileAsync(path);
        yield return abcr;
        request.bundle = abcr.assetBundle;
        if (request.bundle == null)
        {
            Debug.LogError("AssetBundle 加载失败 => " + request.path);
        }

    }
}
