using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Networking;


public class AssetBundleManager : MonoBehaviour
{
    void Start()
    {
#if UNITY_EDITOR
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Tetris Assets/Prefabs/MainTetris.prefab", typeof(GameObject));
        Object.Instantiate(prefab);
#else
        StartCoroutine(LoadAssetBundle());
#endif
    }

    public void Load(string path, GameObject prefab = null)
    {
#if UNITY_EDITOR
        string path1 = Path.Combine("Asset", path);
        prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path1, typeof(GameObject));
        prefab = Object.Instantiate(prefab);
#else
        StartCoroutine(LoadAssetBundle());
#endif
    }
    
    IEnumerator LoadAssetBundle()  
    {  
        string assetBundlePath = Path.Combine(Application.streamingAssetsPath, "tetris");
#if UNITY_ANDROID
        // 对于安卓平台，UnityWebRequest从jar包中加载  
        // string uri = "jar:file://" + Application.dataPath + "!/assets/" + assetBundlePath;  
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(assetBundlePath);  
#else
        // 对于其他平台PC、IOS.....直接从磁盘加载  
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
            Instantiate(prefab);  
            // 实例化预制体  
            // Instantiate(loadedAsset);  
  
            // 卸载AssetBundle  
            bundle.Unload(false);  
        }  
 
    }  

}