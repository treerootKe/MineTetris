using UnityEngine;  
using UnityEditor;  
using System.IO;  
public class BuildAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    public static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/StreamingAssets";  
  
        if (!Directory.Exists(assetBundleDirectory))  
        {  
            Directory.CreateDirectory(assetBundleDirectory);  
        }  
  
        // 设置打包选项: LZ4压缩、无加密
        BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle;  
        // BuildPipeline.BuildAssetBundles(assetBundleDirectory, assetBundleOptions, BuildTarget.Android);
        // string assetBundleDirectory = "Assets/StreamingAssets";
        //
        // if (!System.IO.Directory.Exists(assetBundleDirectory))
        // {
        //     System.IO.Directory.CreateDirectory(assetBundleDirectory);
        // }
        //
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, assetBundleOptions, BuildTarget.StandaloneWindows64);
    }
}
