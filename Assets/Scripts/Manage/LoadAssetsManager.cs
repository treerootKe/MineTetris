using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Manage
{
    public class LoadAssetsManager
    {
        private string assetBundlePath;
        public IEnumerator Load<T>(string path)
            where T : Object
        {
            assetBundlePath = Path.Combine(Application.streamingAssetsPath, path);
#if UNITY_ANDROID
            string uri = "jar:file://" + Application.dataPath + "!/assets/" + assetBundlePath;
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(uri);
#else
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(assetBundlePath);
#endif
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

                // 加载预制体  
                // Object loadedAsset = bundle.LoadAsset<GameObject>("MainTetris");  
                AssetBundleRequest request = bundle.LoadAssetAsync<GameObject>("MainTetris");
                yield return request;

                // 实例化预制体  
                GameObject prefab = request.asset as GameObject;
                Object.Instantiate(prefab);
                // 实例化预制体  
                // Instantiate(loadedAsset);  

                // 卸载AssetBundle  
                bundle.Unload(false);
            }
        }

    }
}
