using UnityEngine;

public class AssetLoadRequest<T> : CustomYieldInstruction where T : Object
{
    public override bool keepWaiting => !LoadFinsh;
    public string BundleName;
    public string AssetName;
    public bool LoadFinsh;
    public T Asset;

    public static AssetLoadRequest<T> Empty = new AssetLoadRequest<T> { LoadFinsh = true };
}
