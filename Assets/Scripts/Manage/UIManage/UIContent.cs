using System;
using UnityEngine;

namespace Manage.UIManage
{
    public struct UIContent
    {
        public Transform traUIParent;
        public string uiPath;
        
        public UIContent(Transform uiParent, string path)
        {
            traUIParent = uiParent;
            uiPath = path;
        }
    }
}