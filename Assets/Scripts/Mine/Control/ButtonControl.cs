using System;
using Mine.DesignPattern;
using Mine.Manage;
using UnityEngine;
using UnityEngine.UI;

namespace Mine.Control
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
        }

        public void EventPause()
        {
            PlayerControl.isPausing = true;
            transformTopPanel.gameObject.SetActive(false);
            transformDirectionKeys.gameObject.SetActive(false);
            transformBottomPanel.gameObject.SetActive(true);
        }

        public void EventStart()
        {
            PlayerControl.isPausing = false;
            transformTopPanel.gameObject.SetActive(true);
            transformDirectionKeys.gameObject.SetActive(true);
            transformBottomPanel.gameObject.SetActive(false);
            if (PlayerControl.globalItemShape == null)
            {
                PlayerControl.Instance.StartTetris();
            }
            else if (PlayerControl.isGameOver)
            {
                for (int i = 0; i < PlayerControl.panelAllBlock.Count; i++) 
                {
                    if (PlayerControl.panelAllBlock[i] != null)
                    {
                        CommonMembers.blockPool.Recycle(PlayerControl.panelAllBlock[i]);
                        PlayerControl.panelAllBlock[i] = null;
                    }
                }
                foreach (var item in PlayerControl.panelAllShape)
                {
                    CommonMembers.shapePool[item.shapeType].Recycle(item);    
                }

                PlayerControl.Instance.txtScore.text = "0";
                PlayerControl.isGameOver = false;
                PlayerControl.panelAllShape.Clear();
                PlayerControl.Instance.StartTetris();
            }
        }
    }
}