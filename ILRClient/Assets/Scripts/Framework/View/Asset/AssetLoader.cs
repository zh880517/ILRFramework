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

    private readonly Dictionary<int, AssetLoadRequest> loadingRequests = new Dictionary<int, AssetLoadRequest>();
    private readonly Queue<int> waits = new Queue<int>();

#if !UNITY_EDITOR && UNITY_ANDROID
    public const int QueueMaxCount = 40;
#else
    public const int QueueMaxCount = 0;
#endif

    private int loadCoroutineCount = 0;

    //Android覆盖安装的时候+1，防止新包加载的时候从缓存加载不从安装包加载，省去加载的时候记录hash和crc的麻烦
    public uint CacheVersion = 0;

    public static void StopAll()
    {
        if (_instance != null)
        {
            _instance.StopAllCoroutines();
            foreach (var kv in _instance.loadingRequests)
            {
                kv.Value.Abort();
            }
            _instance.loadingRequests.Clear();
            _instance.waits.Clear();
            Instance.loadCoroutineCount = 0;
        }
    }

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
        if (QueueMaxCount <= 0 && loadCoroutineCount < QueueMaxCount)
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
        --loadCoroutineCount;
    }

    private IEnumerator LoadAssetBundle(int id, AssetLoadRequest request)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        request.WebLoad = true;
#endif
        string hotPatchPath = ResFileUtil.FindHotPathFilePath(request.path);
        if (hotPatchPath != null)
        {
            request.path = hotPatchPath;
            request.WebLoad = false;
        }
        else
        {
            if (request.WebLoad)
            {
                request.path = ResPathUtil.GetStreamAssetFileUrl(request.path, false);
            }
            else
            {
                request.path = ResPathUtil.GetStreamAssetFilePath(request.path);
            }
        }
        if (request.WebLoad)
        {
            yield return LoadByWebRequest(request);
        }
        else
        {
            yield return LoadByFile(request);
        }
        request.LoadFinish = true;
        loadingRequests.Remove(id);
    }

    private IEnumerator LoadByWebRequest(AssetLoadRequest request)
    {
        UnityWebRequest download = UnityWebRequestAssetBundle.GetAssetBundle(request.path, CacheVersion, 0);
        request.webRequest = download;
        yield return download.SendWebRequest();
        if (download.result == UnityWebRequest.Result.Success)
        {
            request.bundle = DownloadHandlerAssetBundle.GetContent(download);
        }
        request.webRequest = null;
        if (request.bundle == null)
        {
            Debug.LogError("AssetBundle 加载失败 => " + request.path);
        }
    }

    private IEnumerator LoadByFile( AssetLoadRequest request)
    {
        var abcr = AssetBundle.LoadFromFileAsync(request.path);
        yield return abcr;
        request.bundle = abcr.assetBundle;
        if (request.bundle == null)
        {
            Debug.LogError("AssetBundle 加载失败 => " + request.path);
        }
    }
}
