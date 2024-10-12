using System;
using UnityEngine;

namespace Manage.UIManage
{
    public struct UIContent
    {
        public readonly Transform TraUIParent;
        public readonly string UIPath;
        
        public UIContent(Transform uiParent, string path)
        {
            TraUIParent = uiParent;
            UIPath = path;
        }
    }
}