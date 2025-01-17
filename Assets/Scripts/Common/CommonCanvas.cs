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
        public static Transform TraGameMenuCanvas;  //菜单UI界面
        public static Transform TraGameCanvas;      //游戏主体UI界面
        public static Transform TraGameUICanvas;    //游戏UI界面

        private void Awake()
        {
            FindComponent();
        }

        private void FindComponent()
        {
            TraGameMenuCanvas = transform.Find("GameMenuCanvas");
            TraGameCanvas = transform.Find("GameCanvas");
            TraGameUICanvas = transform.Find("GameUICanvas");
        }
    }
}
