using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorBundle : IBundle
{
    private readonly string name;
    private bool isFinish;
    private Dictionary<string, string> nameToPath = new Dictionary<string, string>();

    public string Name => name;

    public bool LoadFinish => isFinish;

    public EditorBundle(string name, IEnumerable<string> files)
    {
        this.name = name;
        foreach (var file in files)
        {
            nameToPath.Add(System.IO.Path.GetFileNameWithoutExtension(file), file);
        }
    }

    public void AddDepend(IBundle bundle)
    {
        
    }

    public IEnumerator Load()
    {
        if (!isFinish)
        {
            isFinish = true;
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator LoadAsset<T>(string asset, int key, Action<T, int, string> func) where T : UnityEngine.Object
    {
        //防止部分依赖异步处理的逻辑直接回调造成错误
        yield return new WaitForEndOfFrame();
        T obj = default;
        if (nameToPath.TryGetValue(asset, out string filePath))
        {
#if UNITY_EDITOR
            obj = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(filePath);
#endif
        }
        if (obj == null)
        {
            Debug.LogErrorFormat("{0}不存在, Type = {1}, Bundle = {2}", asset, typeof(T), name);
        }
        func.Invoke(obj, key, asset);
    }

    public void UnLoad()
    {
        nameToPath.Clear();
    }
}
