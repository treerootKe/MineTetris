using UnityEditor;
using UnityEngine;

namespace Tetris.Manage.UIManage
{
    public class UIManager
    {
        public static void ShowUI(UIContent content)
        {
            // 使用AssetDatabase来加载预制体  
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(content.uiPath);
            prefab.transform.SetParent()
        }
    }
}
