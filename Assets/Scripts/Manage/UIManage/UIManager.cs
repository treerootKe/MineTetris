using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Manage.UIManage
{
    public class UIManager
    {
        private static Dictionary<UIContent,GameObject> dicUIPrefab = new Dictionary<UIContent, GameObject>();
        public static void ShowUI(UIContent content)
        {
            if (dicUIPrefab.ContainsKey(content))
            {
                dicUIPrefab[content].SetActive(true);
                return;
            }
            // 使用AssetDatabase来加载预制体
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(content.uiPath);
            GameObject instance = Object.Instantiate(prefab, content.traUIParent);  
            dicUIPrefab.Add(content, instance);
        }

        public static void CloseUI(UIContent content)
        {
            if (dicUIPrefab.ContainsKey(content))
            {
                dicUIPrefab[content].SetActive(false);
            }
            else
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(content.uiPath);
                GameObject instance = Object.Instantiate(prefab, content.traUIParent);
                dicUIPrefab.Add(content, instance);
                dicUIPrefab[content].SetActive(false);
            }
        }
    }
}
