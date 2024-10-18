using Common;
using UnityEngine;

namespace Manage.LoadAssetsManage
{
    public struct PrefabParent
    {
        public static Transform Game => CommonCanvas.traGameCanvas;

        public static Transform GameUI => CommonCanvas.traGameUICanvas;

        public static Transform GameMenu => CommonCanvas.traGameMenuCanvas;
    }
}