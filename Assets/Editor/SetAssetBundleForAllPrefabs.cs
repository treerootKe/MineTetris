using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class SetAssetBundleForAllPrefabs  
    {  
        [MenuItem("Assets/Set AssetBundle for All Prefabs in HotUpdateResources")]  
        static void SetAssetBundleForAllPrefabsInHotResources()  
        {  
            string targetRootFolder = "Assets/HotUpdateResources";  
  
            // 递归获取目标文件夹及其子文件夹下的所有预制体  
            string[] allPrefabFiles = Directory.GetFiles(targetRootFolder, "*.prefab", SearchOption.AllDirectories); 
            // string[] strsAllPrefabGuid = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets" });
            // string[] strsAllPrefabs = new string[strsAllPrefabGuid.Length];
            // for (int i = 0; i < strsAllPrefabs.Length; i++)
            // {
            //     strsAllPrefabs[i] = AssetDatabase.GUIDToAssetPath(strsAllPrefabGuid[i]);
            // }
            foreach (string prefabPath in allPrefabFiles)  
            {  
                // 获取资源的Importer  
                AssetImporter assetImporter = AssetImporter.GetAtPath(prefabPath);  
                if (assetImporter != null)  
                {  
                    // 设置AssetBundle名称  
                    string assetBundleName = GetAssetBundleNameFromPath(prefabPath, targetRootFolder);  
                    assetImporter.assetBundleName = assetBundleName;  
  
                    Debug.Log($"Set AssetBundle for: {prefabPath} to {assetBundleName}");  
                }  
            }  
  
            // 保存更改  
            AssetDatabase.SaveAssets();  
            AssetDatabase.Refresh();  
        }  
  
        static string GetAssetBundleNameFromPath(string prefabPath, string rootFolder)  
        {  
            // 去掉前缀"Assets/"和根文件夹路径  
            string relativePath = prefabPath.Replace(rootFolder, "").Replace("Assets/", "").TrimStart('\\');  
            
            // 获取文件夹部分作为AssetBundle名称  
            string directoryPath = Path.GetDirectoryName(relativePath);  
            
            // 将路径中的斜杠替换为下划线，并转换为小写  
            return directoryPath.Replace("\\", "_").ToLower();  
        }  
    }
}