using System;
using System.Collections;
using System.Collections.Generic;
using Tetris;
using Tetris.Control;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Manage.LoadAssetsManage
{
    public class LoadManager<T>:MonoBehaviour where T:Object
    {
        private static readonly Dictionary<AssetContent, T> DicAssets = new Dictionary<AssetContent, T>();
        private static readonly Dictionary<AssetContent,GameObject> DicPrefabs = new Dictionary<AssetContent, GameObject>();
        public static IEnumerator ShowPrefab(AssetContent content)
        {
            if (DicAssets.TryGetValue(content,out var prefab))
            {
                DicPrefabs[content]?.SetActive(true);
                yield break;
            }
            yield return AssetBundleManager<T>.LoadAsset(content, (contentCallback, asset) =>
            {
                DicAssets.Add(content, asset);
                var assetPrefab = asset as GameObject;
                var instance = Instantiate(assetPrefab,contentCallback.TraParent);
                instance.SetActive(true);
                DicPrefabs.Add(contentCallback, instance);
            });
        }
        
        public static void ClosePrefab(AssetContent content)
        {
            if (DicAssets.TryGetValue(content,out var prefab))
            {
                DicPrefabs[content].SetActive(false);
            }
            else
            {
                Debug.Log("Can't find prefab");
            }
        }

        public static T LoadAsset<T>(AssetContent content) where T : Object
        {
            // T asset = AssetDatabase.LoadAssetAtPath<T>(content.StrPath);
            return null;
        }
    }
}
