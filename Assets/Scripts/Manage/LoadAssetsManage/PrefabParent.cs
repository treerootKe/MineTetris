using Common;
using UnityEngine;

namespace Manage.LoadAssetsManage
{
    public struct PrefabParent
    {
        public static Transform Game => CommonCanvas.S_TraGameCanvas;

        public static Transform GameUI => CommonCanvas.S_TraGameUICanvas;

        public static Transform GameMenu => CommonCanvas.S_TraGameMenuCanvas;
    }
}