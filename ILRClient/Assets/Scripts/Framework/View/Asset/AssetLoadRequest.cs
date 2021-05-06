using UnityEngine.Networking;
using UnityEngine;

public class AssetLoadRequest : CustomYieldInstruction
{
    public AssetBundle bundle;
    public string path;
    public bool LoadFinish;
    public bool WebLoad;
    public UnityWebRequest webRequest;
    public override bool keepWaiting => !LoadFinish;

    public void Abort()
    {
        if (webRequest != null)
        {
            webRequest.Abort();
            webRequest = null;
        }
    }
}
