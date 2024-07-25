using UnityEditor;
using UnityEngine;

namespace Tetris.Manage.UIManage
{
    public class UIManager
    {
        public static void ShowUI(UIContent content)
        {
            // ʹ��AssetDatabase������Ԥ����  
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(content.uiPath);
            prefab.transform.SetParent()
        }
    }
}
