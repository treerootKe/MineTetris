using Common;
using UnityEngine;

namespace Manage.LoadAssetsManage
{
    public struct PrefabParent
    {
        public static Transform Game => CommonCanvas.TraGameCanvas;

        public static Transform GameUI => CommonCanvas.TraGameUICanvas;

        public static Transform GameMenu => CommonCanvas.TraGameMenuCanvas;
    }
}