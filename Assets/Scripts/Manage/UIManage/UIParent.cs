using Common;
using UnityEngine;

namespace Manage.UIManage
{
    public struct UIParent
    {
        public static Transform Game
        {
            get
            {
                return CommonCanvas.traGameCanvas;
            }
        }
        public static Transform GameUI
        {
            get
            {
                return CommonCanvas.traGameUICanvas;
            }
        }
        public static Transform GameMenu
        {
            get
            {
                return CommonCanvas.traGameMenuCanvas;
            }
        }
    }
}