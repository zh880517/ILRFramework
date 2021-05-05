using SharpRecast;
using UnityEditor;
using UnityEngine;

public static class SDFTest
{

    [MenuItem("Tools/生成SDF贴图")]
    public static void Test()
    {
        if (Selection.activeObject is GameObject go)
        {
            var pts = GameObject.Find("Points");
            var pt = pts.transform.GetChild(0).position;
            var sdf = RecastBuilder.SceneToSDF(go, pt);
            Texture2D texture = new Texture2D(100, 100);
            RecastBuilder.SDFRenderToTexture(sdf, texture);
            //Texture2D texture = RecastBuilder.SDFToTexture(sdf);
            var bytes = texture.EncodeToPNG();
            System.IO.File.WriteAllBytes("Assets/sdf.png", bytes);
            Object.DestroyImmediate(texture);
        }
    }

    private static Texture2D GridToTexture(RecastGridData grid)
    {
        Texture2D texture = new Texture2D(grid.Width, grid.Length);
        for (int i = 0; i < grid.Width; ++i)
        {
            for (int j = 0; j < grid.Length; ++j)
            {
                if (grid.Mask.Get(i + j*grid.Width))
                {
                    texture.SetPixel(i, j, Color.white);
                }
                else
                {
                    texture.SetPixel(i, j, Color.black);
                }
            }
        }
        return texture;
    }
}
