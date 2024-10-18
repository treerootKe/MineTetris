using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    public static void BuildAllAssetBundles()
    {
        // string assetBundleDirectory = "Assets/StreamingAssets";  
        //
        // if (!Directory.Exists(assetBundleDirectory))  
        // {  
        //     Directory.CreateDirectory(assetBundleDirectory);  
        // }  
        //
        // // 设置打包选项: LZ4压缩、无加密
        // BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle;  
        // BuildPipeline.BuildAssetBundles(assetBundleDirectory, assetBundleOptions, BuildTarget.StandaloneWindows64);
        BuildTarget[] targetPlatforms = {
            BuildTarget.StandaloneWindows64, 
            BuildTarget.Android, 
        };

        foreach (var target in targetPlatforms)
        {
            string assetBundleDirectory = "Assets/StreamingAssets" + target;

            if (!Directory.Exists(assetBundleDirectory))
            {
                Directory.CreateDirectory(assetBundleDirectory);
            }

            BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle;
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, assetBundleOptions, target);

            Debug.Log("AssetBundles built for platform: " + target);
        }
    }
}
