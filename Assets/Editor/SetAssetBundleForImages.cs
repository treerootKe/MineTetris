using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class SetAssetBundleForImages : MonoBehaviour
    {
        [MenuItem("Assets/Set AssetBundle for Images")]  
        static void SetAssetBundleForAllImages()  
        {  
            string[] imageExtensions = new string[] { ".png", ".jpg", ".jpeg", ".tga", ".gif" };  
  
            // 获取项目中所有的资源路径  
            string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();  
  
            foreach (string assetPath in allAssetPaths)  
            {  
                // 检查文件扩展名是否为图片  
                if (IsImageFile(assetPath, imageExtensions))  
                {  
                    // 获取资源的Importer  
                    AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);  
  
                    if (assetImporter != null)  
                    {  
                        // 设置AssetBundle名称  
                        string assetBundleName = "images";  // 这里可以根据需要设置不同的名称  
                        assetImporter.assetBundleName = assetBundleName;  
  
                        Debug.Log($"Set AssetBundle for: {assetPath} to {assetBundleName}");  
                    }  
                }  
            }  
  
            // 保存更改  
            AssetDatabase.SaveAssets();  
            AssetDatabase.Refresh();  
        }  
  
        static bool IsImageFile(string path, string[] extensions)  
        {  
            string fileExtension = Path.GetExtension(path).ToLower();  
            foreach (string ext in extensions)  
            {  
                if (fileExtension == ext)  
                {  
                    return true;  
                }  
            }  
            return false;  
        }  
    }
}
