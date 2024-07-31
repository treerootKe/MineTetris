using DG.Tweening;
using Manage.UIManage;
using System;
using Tetris.Common;
using DesignPattern;
using Tetris.Manage;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.Control
{
    public class ButtonControl:MonoSingleton<ButtonControl>
    {
        public Button btnPause;
        
        public Button btnStart;
        public Button btnRestart;
        public Button btnLevel;
        public Button btnSetting;

        public Button btnA;
        public Button btnB;
        public Button btnDown;
        public Button btnLeft;
        public Button btnRight;

        //玩家控制对象实例
        public UIMainTetrisControl mainInstance;

        protected override void Awake()
        {
            base.Awake();
            FindComponent();
            RegisterButtonEvent();
        }

        private void FindComponent()
        {
            btnPause = transform.Find("TopPanel/btnPause").GetComponent<Button>();

            btnStart = transform.Find("BottomPanel/btnStart").GetComponent<Button>();
            btnRestart = transform.Find("BottomPanel/btnRestart").GetComponent<Button>();
            btnLevel = transform.Find("BottomPanel/btnLevel").GetComponent<Button>();
            btnSetting = transform.Find("BottomPanel/btnSetting").GetComponent<Button>();
            
            btnA = transform.Find("DirectionKeys/btnA").GetComponent<Button>();
            btnB = transform.Find("DirectionKeys/btnB").GetComponent<Button>();
            btnDown = transform.Find("DirectionKeys/btnDown").GetComponent<Button>();
            btnLeft = transform.Find("DirectionKeys/btnLeft").GetComponent<Button>();
            btnRight = transform.Find("DirectionKeys/btnRight").GetComponent<Button>();

            mainInstance = UIMainTetrisControl.Instance;
        }

        private void RegisterButtonEvent()
        {
            btnLeft.onClick.AddListener(()=>TetrisEventManager.eventShapeMoveX?.Invoke(ShapeChange.Left));
            btnRight.onClick.AddListener(()=>TetrisEventManager.eventShapeMoveX?.Invoke(ShapeChange.Right));
            btnA.onClick.AddListener(() => TetrisEventManager.eventShapeRotate?.Invoke(ShapeChange.RotateA));
            btnB.onClick.AddListener(() => TetrisEventManager.eventShapeRotate?.Invoke(ShapeChange.RotateB));
            btnDown.onClick.AddListener(()=>TetrisEventManager.eventDropFastest?.Invoke());
            
            btnPause.onClick.AddListener(() => TetrisEventManager.eventPauseGame?.Invoke());
            btnStart.onClick.AddListener(() => TetrisEventManager.eventStartGame?.Invoke());
            btnRestart.onClick.AddListener(() => TetrisEventManager.eventRestartGame?.Invoke());
            btnLevel.onClick.AddListener(() => UIManager.ShowUI(UIPath.UIChooseLevel));
        }
    }
}