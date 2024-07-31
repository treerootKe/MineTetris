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
        public static Transform traGameMenuCanvas;  //菜单UI界面
        public static Transform traGameCanvas;      //游戏主体UI界面
        public static Transform traGameUICanvas;    //游戏UI界面

        private void Awake()
        {
            FindComponent();
        }

        private void FindComponent()
        {
            traGameMenuCanvas = transform.Find("GameMenuCanvas");
            traGameCanvas = transform.Find("GameCanvas");
            traGameUICanvas = transform.Find("GameUICanvas");
        }
    }
}
