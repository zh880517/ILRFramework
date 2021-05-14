using UnityEditor;
using UnityEngine;

public static class EditorUttils
{
    [MenuItem("Assets/输出类型名字")]
    static void PrintAssetType()
    {
        Debug.Log(AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]));
        if (Selection.activeObject)
        {
            Debug.Log(Selection.activeObject.GetType().FullName);
        }
    }
}
