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
    public class AssetBundleManager<T> where T : Object
    {
        private static string assetName;

        public static IEnumerator LoadAsset(AssetContent content,Action<AssetContent, T> callback)
        {
            assetName = Path.GetFileName(content.StrPath);
#if UNITY_EDITOR
            string path1 = new StringBuilder("Assets/HotUpdateResources/").Append(PlayerData.gamesName.ToString()).Append("/").Append(content.StrType).Append("/").Append(content.StrPath).ToString();
            Debug.Log("路径：" + path1);
            T asset = (T)AssetDatabase.LoadAssetAtPath(path1, typeof(T));
            callback(content, asset);
#else
            yield return LoadAssetBundle(content, callback);
#endif
            yield return null;
        }

        IEnumerator LoadAssetBundle(AssetContent content,Action<AssetContent, T> callback)
        {
            string assetBundleName;
            string assetPath = content.StrType.Replace("_", "/");
            string streamingAssetsPath = Application.streamingAssetsPath;
#if UNITY_ANDROID
        // 对于安卓平台，UnityWebRequest从jar包中加载  
//         assetBundleName = Path.Combine(streamingAssetsPath, "Android");
//         string uri = "jar:file://" + Application.dataPath + "!/assets/" + assetBundleName + assetPath;
//         UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(uri);
// #else
            // 对于其他平台PC、IOS.....直接从磁盘加载  
            assetBundleName = Path.Combine(streamingAssetsPath, "StandaloneWindows64");
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(assetBundleName + assetPath);
#endif
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Asset loading failed:" + www.error);
            }
            else
            {
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

                // 从.bundle文件中加载资源
                // Object loadedAsset = bundle.LoadAsset<GameObject>("MainTetris");  
                AssetBundleRequest request = bundle.LoadAssetAsync<T>(assetName);
                yield return request;

                T asset = request.asset as T;
                callback(content, asset);
                // 卸载AssetBundle  
                bundle.Unload(false);
            }
        }
    }
}