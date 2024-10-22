using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using Common;
using Manage.LoadAssetsManage;
using UnityEditor;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Tetris
{
    public class AssetBundleManager
    {
        private static string _assetName;

        public static IEnumerator LoadAsset<T>(AssetContent content,Action<AssetContent, T> callback) where T : Object
        {
            _assetName = Path.GetFileName(content.StrPath);
#if UNITY_EDITOR
            // string path1 = new StringBuilder("Assets/HotUpdateResources/").Append(PlayerData.gamesName.ToString()).Append("/").Append(content.StrType).Append("/").Append(content.StrPath).ToString();
            // Debug.Log("路径：" + path1);
            // T asset = (T)AssetDatabase.LoadAssetAtPath(path1, typeof(T));
            // callback(content, asset);
// #else
            yield return LoadAssetBundle<T>(content, callback);
#endif
            yield return null;
        }

        static IEnumerator  LoadAssetBundle<T>(AssetContent content,Action<AssetContent, T> callback) where T : Object
        {
            string assetBundleName;
            string assetPath = PlayerData.gamesName + "_" + content.StrType + "_" + Path.GetDirectoryName(content.StrPath)?.Replace('\\', '_');
            assetPath = assetPath.ToLower();
#if UNITY_ANDROID
        //对于安卓平台，UnityWebRequest从jar包中加载  
        assetBundleName = Application.streamingAssetsPath + "/" + "Android";
        Debug.Log("路径：" + assetBundleName + "/" + assetPath);
        string uri = "jar:file://" + Application.dataPath + "!/assets/" + assetBundleName + "/" + assetPath;
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(uri);
        // UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(assetBundleName + "/" + assetPath);
#else
            // 对于其他平台PC、IOS.....直接从磁盘加载
            assetBundleName = Application.streamingAssetsPath + "/" + "StandaloneWindows64";
            Debug.Log("路径：" + assetBundleName + "/" + assetPath);
            UnityWebRequest www =
                UnityWebRequestAssetBundle.GetAssetBundle(assetBundleName + "/" + assetPath);

#endif
            yield return www.SendWebRequest();

            // if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            // {
            //     Debug.LogError("Asset loading failed:" + www.error);
            // }
            // else
            // {
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

                // 从.bundle文件中加载资源
                // Object loadedAsset = bundle.LoadAsset<GameObject>("MainTetris");  
                AssetBundleRequest request = bundle.LoadAssetAsync<T>(_assetName);
                yield return request;

                T asset = request.asset as T;
                callback(content, asset);
                // 卸载AssetBundle  
                bundle.Unload(false);
            // }
        }
    }
}