using UnityEngine;

public class AssetLoadRequest : CustomYieldInstruction
{
    public AssetBundle bundle;
    public string path;
    public override bool keepWaiting => bundle == null;
}
