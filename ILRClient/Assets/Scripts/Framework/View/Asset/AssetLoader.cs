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

    private readonly Dictionary<int, AssetBundleLoadRequest> loadingRequests = new Dictionary<int, AssetBundleLoadRequest>();
    private readonly Queue<int> waits = new Queue<int>();
    private readonly Queue<System.Func<IEnumerator>> asyncTasks = new Queue<System.Func<IEnumerator>>();

    public const int QueueMaxCount = 40;

    private int loadCoroutineCount = 0;
    private int taskCoroutineCount = 0;

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
            _instance.asyncTasks.Clear();
            Instance.loadCoroutineCount = 0;
        }
    }

    public AssetBundleLoadRequest Load(int hasId,string path)
    {
        if (loadingRequests.TryGetValue(hasId, out var request))
        {
            return request;
        }
        request = new AssetBundleLoadRequest
        {
            path = path
        };
        loadingRequests.Add(hasId, request);
        waits.Enqueue(hasId);
        if (loadCoroutineCount < QueueMaxCount)
        {
            StartCoroutine(DoLoad());
        }
        return request;
    }

    public void AddTask(System.Func<IEnumerator> task)
    {
        asyncTasks.Enqueue(task);
        if (taskCoroutineCount < QueueMaxCount)
        {
            StartCoroutine(DoTask());
        }
    }

    private IEnumerator DoTask()
    {
        ++taskCoroutineCount;
        while (asyncTasks.Count > 0)
        {
            var task = asyncTasks.Dequeue();
            if (task != null)
            {
                yield return task();
            }
        }
        --taskCoroutineCount;
    }

    private IEnumerator DoLoad()
    {
        ++loadCoroutineCount;
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

    private IEnumerator LoadAssetBundle(int id, AssetBundleLoadRequest request)
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

    private IEnumerator LoadByWebRequest(AssetBundleLoadRequest request)
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

    private IEnumerator LoadByFile( AssetBundleLoadRequest request)
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
