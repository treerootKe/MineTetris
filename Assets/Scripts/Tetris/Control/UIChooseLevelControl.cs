using System;
using Manage.UIManage;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.Control
{
    public class UIChooseLevelControl:MonoBehaviour
    {
        public Toggle[] levelToggles;
        public Button btnClose;

        private void Awake()
        {
            FindComponent();
            for (int i = 0; i < levelToggles.Length; i++)
            {
                var i1 = i;
                levelToggles[i].onValueChanged.AddListener(arg0 =>
                {
                    if (arg0)
                    {
                        UIMainTetrisControl.Instance.nDropIntervalLevel = i1;
                        UIMainTetrisControl.Instance.txtLevel.text = (i1 + 1).ToString();
                        UIMainTetrisControl.Instance.fDropInterval = UIMainTetrisControl.Instance.fDropIntervals[i1];
                    }
                });
            }
            btnClose.onClick.AddListener(() => UIManager.CloseUI(UIPath.UIChooseLevel));
        }

        private void FindComponent()
        {
            levelToggles = new Toggle[3];
            for (int i = 0; i < 3; i++)
            {
                levelToggles[i] = transform.Find("imgBackGround/toggle").GetChild(i).GetComponent<Toggle>();
            }

            btnClose = transform.Find("imgBackGround/btnClose").GetComponent<Button>();
        }
    }
}