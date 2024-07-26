using Manage.UIManage;
using System;
using Tetris.Common;
using Tetris.DesignPattern;
using Tetris.Manage;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.Control
{
    public class ButtonControl:MonoSingleton<ButtonControl>
    {
        public Transform transformTopPanel;
        public Transform transformBottomPanel;
        public Transform transformDirectionKeys;
        
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

        protected override void Awake()
        {
            base.Awake();
            FindComponent();
            RegisterButtonEvent();
        }

        private void FindComponent()
        {
            transformTopPanel = transform.Find("TopPanel");
            transformBottomPanel = transform.Find("BottomPanel");
            transformDirectionKeys = transform.Find("DirectionKeys");
            
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
        }

        private void RegisterButtonEvent()
        {
            btnLeft.onClick.AddListener(()=>EventManager.eventShapeMoveX?.Invoke(ShapeChange.Left));
            btnRight.onClick.AddListener(()=>EventManager.eventShapeMoveX?.Invoke(ShapeChange.Right));
            btnA.onClick.AddListener(() => EventManager.eventShapeRotate?.Invoke(ShapeChange.RotateA));
            btnB.onClick.AddListener(() => EventManager.eventShapeRotate?.Invoke(ShapeChange.RotateB));
            btnDown.onClick.AddListener(()=>EventManager.eventDropFastest?.Invoke());
            
            btnPause.onClick.AddListener(EventPause);
            btnStart.onClick.AddListener(EventStart);
            btnLevel.onClick.AddListener(() => UIManager.ShowUI(UIPath.UIChooseLevel));
        }

        public void EventPause()
        {
            UIMainTetrisControl.isPausing = true;
            transformTopPanel.gameObject.SetActive(false);
            transformDirectionKeys.gameObject.SetActive(false);
            transformBottomPanel.gameObject.SetActive(true);
        }

        public void EventStart()
        {
            UIMainTetrisControl.isPausing = false;
            transformTopPanel.gameObject.SetActive(true);
            transformDirectionKeys.gameObject.SetActive(true);
            transformBottomPanel.gameObject.SetActive(false);
            if (UIMainTetrisControl.globalItemShape == null)
            {
                UIMainTetrisControl.Instance.OnceDropInit();
            }
            else if (UIMainTetrisControl.isGameOver)
            {
                for (int i = 0; i < UIMainTetrisControl.panelAllBlock.Count; i++) 
                {
                    if (UIMainTetrisControl.panelAllBlock[i] != null)
                    {
                        TetrisCommonMembers.blockPool.Recycle(UIMainTetrisControl.panelAllBlock[i]);
                        UIMainTetrisControl.panelAllBlock[i] = null;
                    }
                }
                foreach (var item in UIMainTetrisControl.panelAllShape)
                {
                    TetrisCommonMembers.shapePool[item.shapeType].Recycle(item);    
                }

                UIMainTetrisControl.Instance.txtScore.text = "0";
                UIMainTetrisControl.isGameOver = false;
                UIMainTetrisControl.panelAllShape.Clear();
                UIMainTetrisControl.Instance.OnceDropInit();
            }
        }
    }
}