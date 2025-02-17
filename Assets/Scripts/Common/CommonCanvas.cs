using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Common
{
    public class CommonCanvas:MonoBehaviour
    {
        public static Transform S_TraGameMenuCanvas;  //菜单UI界面
        public static Transform S_TraGameCanvas;      //游戏主体UI界面
        public static Transform S_TraGameUICanvas;    //游戏UI界面

        private void Awake()
        {
            FindComponent();
        }

        private void FindComponent()
        {
            S_TraGameMenuCanvas = transform.Find("GameMenuCanvas");
            S_TraGameCanvas = transform.Find("GameCanvas");
            S_TraGameUICanvas = transform.Find("GameUICanvas");
        }
    }
}
