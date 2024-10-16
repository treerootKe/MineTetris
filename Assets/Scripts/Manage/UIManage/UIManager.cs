using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Manage.UIManage
{
    public class UIManager
    {
        private static readonly Dictionary<UIContent,GameObject> DicUIPrefab = new Dictionary<UIContent, GameObject>();
        public static void ShowUI(UIContent content)
        {
            if (DicUIPrefab.TryGetValue(content, out var value))
            {
                value.SetActive(true);
                return;
            }
            // ʹ��AssetDatabase������Ԥ����
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(content.UIPath);
            GameObject instance = Object.Instantiate(prefab, content.TraUIParent);  
            DicUIPrefab.Add(content, instance);
        }

        public static void CloseUI(UIContent content)
        {
            if (DicUIPrefab.TryGetValue(content, out var value))
            {
                value.SetActive(false);
            }
            else
            {
                // GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(content.UIPath);
                // GameObject instance = Object.Instantiate(prefab, content.TraUIParent);
                // DicUIPrefab.Add(content, instance);
                // DicUIPrefab[content].SetActive(false);
            }
        }
    }
}
