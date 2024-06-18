using System.Collections;
using UnityEngine;
using Tetris.ObjectPoolItem;
using System.Collections.Generic;
using DG.Tweening;
using Tetris.ToolClasses;
using Tetris.Common;
using Tetris.DesignPattern;
using UnityEngine.UI;
using Tetris.Manage;
using Control;

namespace Tetris.Control
{
    public  enum ShapeChange
    {
        Left,
        Right,
        RotateA,
        RotateB
    }
    public class UIMainTetrisControl:MonoSingleton<UIMainTetrisControl>
    {
        public Transform transformDropPanel;        //下落区域的父物体
        public Transform transformPrefab;           //预制体的父物体
        public static List<Transform> panelAllBlock;//下落区域中的200个方块
        public static List<ItemShape> panelAllShape;//下落区域中，所有的形状
        public GameObject[] gameObjectsNextShape;   //下一个形状的游戏物体数组
        public static ItemShape globalItemShape;    //当前正在下落的形状

        private int _mScore;                        //当前总得分
        public bool isFastDrop;                     //形状是否可以进行快速下落                  
        private int _mNextShape;                    //下一个形状的索引(0--6，分别对应7种形状)
        public float fDropInterval;                 //形状当前下落间隔
        public float[] fDropIntervals;              //形状每下落一次，需要等待间隔的数组
        public int nDropIntervalLevel;              //形状下落间隔等级(0--2)
        public float fDropIntervalFastest;          //形状快速下落时的等待间隔
        public static bool isPausing;               //是否正在暂停
        public static bool isGameOver;              //是否游戏结束
        
        public Text txtScore;                       //分数显示文本
        public Text txtHistoryScore;                //历史分数显示文本
        public Text txtLevel;                       //形状下落间隔等级显示文本

        private IEnumerator _mIEBlockDrop;           //形状下落的协程
        protected override void Awake()
        {
            base.Awake();
            FindComponent();
            InitValues();
        }

        private void InitValues()
        {
            TetrisCommonMembers.InitValue();
            panelAllBlock = new List<Transform>();
            for (int i = 0; i < 210; i++)
            {
                panelAllBlock.Add(null);
            }

            panelAllShape = new List<ItemShape>();
            //初始化对象池
            TetrisCommonMembers.shapePool = new ObjectPool<ItemShape>[7];
            for (int i = 0; i < 7; i++)
            {
                TetrisCommonMembers.shapePool[i] = new ObjectPool<ItemShape>(transformPrefab.GetChild(i).GetComponent<ItemShape>());
            }
            TetrisCommonMembers.blockPool = new ObjectPool<Transform>(transformPrefab.Find("block"));
            fDropIntervals = new float[3] { 1, 0.5f, 0.25f };
            fDropIntervalFastest = 0.05f;
            fDropInterval = fDropIntervals[nDropIntervalLevel];
            _mNextShape = UnityEngine.Random.Range(0, 7);
        }
        //获取组件
        private void FindComponent()
        {
            transformDropPanel = transform.Find("BlockDropArea/DropPanel");
            transformPrefab = transform.Find("Prefab");
            txtScore = transform.Find("ScoreArea/imgScore/txtScore").GetComponent<Text>();
            txtHistoryScore = transform.Find("ScoreArea/imgScore/txtScore").GetComponent<Text>();
            txtLevel = transform.Find("ScoreArea/imgScore/txtScore").GetComponent<Text>();
            gameObjectsNextShape = new GameObject[7];
            for (int i = 0; i < 7; i++)
            {
                gameObjectsNextShape[i] = transform.Find("ScoreArea/imgNextShape").GetChild(i).gameObject;
            }
        }
        private void OnEnable()
        {
            EventManager.eventShapeMoveX += EventShapeMoveX;
            EventManager.eventDropFastest += EventDropFastest;
            EventManager.eventShapeRotate += EventShapeRotate;
        }

        private void OnDisable()
        {
            EventManager.eventShapeMoveX -= EventShapeMoveX;
            EventManager.eventDropFastest -= EventDropFastest;
            EventManager.eventShapeRotate -= EventShapeRotate;
        }

        private void Update()
        {
            if (!isGameOver)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    EventManager.eventShapeRotate?.Invoke(ShapeChange.RotateA);
                }
                if (Input.GetKeyDown(KeyCode.B))
                {
                    EventManager.eventShapeRotate?.Invoke(ShapeChange.RotateB);
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    EventManager.eventShapeMoveX?.Invoke(ShapeChange.Left);
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    EventManager.eventShapeMoveX?.Invoke(ShapeChange.Right);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    EventManager.eventDropFastest?.Invoke();
                }
            }
        }
        
        //生成一个新的形状前，初始化一下数据
        public void OnceDropInit()
        {
            isFastDrop = true;
            gameObjectsNextShape[_mNextShape].SetActive(false);
            globalItemShape = TetrisCommonMembers.shapePool[_mNextShape].Get(transformDropPanel);
            panelAllShape.Add(globalItemShape);
            _mNextShape = UnityEngine.Random.Range(0, 7);
            gameObjectsNextShape[_mNextShape].SetActive(true);
            _mIEBlockDrop = BlockDrop(globalItemShape);
            StartTetris();
        }

        public void StartTetris()
        {
            if (!globalItemShape.JudgeIsPossibleDrop(panelAllBlock))
            {
                Debug.Log("game over");
                isGameOver = true;
                ButtonControl.Instance.EventPause();
                return;
            }
            StartCoroutine(_mIEBlockDrop);
        }

        IEnumerator BlockDrop(ItemShape item,bool isRecursion = false)
        {
            yield return new WaitForSeconds(fDropInterval);
            while (item.JudgeIsPossibleDrop(panelAllBlock))
            {
                while (isPausing)
                {
                    yield return null;
                }
                item.transform.DOLocalMoveY(item.transform.localPosition.y - 45, 0.001f).OnComplete(item.SetBlockPos);
                AudioController.Instance.PlaySound("Drop");
                yield return new WaitForSeconds(fDropInterval);
            }
            yield return new WaitForSeconds(0.1f);
            if (item.JudgeIsPossibleDrop(panelAllBlock))
            {
                yield return BlockDrop(item, true);
            }
            if (isRecursion)
            {
                yield break;
            }
            for (int i = 0; i < 4; i++)
            {
                panelAllBlock[item.blockPos[i]] = item.fourBlock[i];
            }
            fDropInterval = fDropIntervals[nDropIntervalLevel];
            yield return BlockDisappear();
            while (isPausing)
            {
                yield return null;
            }
            globalItemShape = null;
            OnceDropInit();
        }

        IEnumerator BlockDisappear()
        {
            List<int> disappearRow = new List<int>();
            for (int i = 0; i < 200; i += 10) 
            {
                bool isNeedDisappear = true;
                for (int j = i; j < i + 10; j++)
                {
                    if (panelAllBlock[j] == null)
                    {
                        isNeedDisappear = false;
                        break;
                    }
                }
                if (isNeedDisappear) 
                {
                    disappearRow.Add(i);
                    for (int j = i; j < i + 10; j++)
                    {
                        TetrisCommonMembers.blockPool.Recycle(panelAllBlock[j]);
                        panelAllBlock[j] = null;
                    }
                }
            }
            //加分，然后更新下落面板中存储方块的集合
            if (disappearRow.Count != 0)
            {
                AddScore(disappearRow.Count);
                for (int i = disappearRow[disappearRow.Count - 1] + 10; i < 200; i++)
                {
                    if (panelAllBlock[i] != null)
                    {
                        panelAllBlock[i].DOLocalMoveY(panelAllBlock[i].transform.localPosition.y - 45 * disappearRow.Count, 0.001f);
                        panelAllBlock[i - disappearRow.Count * 10] = panelAllBlock[i];
                        panelAllBlock[i] = null;
                    }
                }

                for (int i = 0; i < panelAllShape.Count; i++)
                {
                    if ( panelAllShape[i].RecycleShape())
                    {
                        panelAllShape.RemoveAt(i);
                    }
                }
                AudioController.Instance.PlaySound("Disappear");
                yield return new WaitForSeconds(1f);
            }
        }
        private void AddScore(int nDisappearLine)
        {
            int nOnceScore = nDisappearLine * nDisappearLine * 100;
            DOTween.To(endValue =>
            {
                txtScore.text = ((int)endValue).ToString();
            }, _mScore, _mScore + nOnceScore, 1).OnComplete(() => _mScore += nOnceScore);
        }

        #region 事件
        private void EventShapeMoveX(ShapeChange moveDirection)
        {
            if (globalItemShape.JudgeIsPossibleMoveX(panelAllBlock, moveDirection))
            {
                var transform1 = globalItemShape.transform;
                var pos = transform1.localPosition;
                switch (moveDirection)
                {
                    case ShapeChange.Left:
                        transform1.localPosition = new Vector3(pos.x - 45, pos.y, pos.z);
                        break;
                    case ShapeChange.Right:
                        transform1.localPosition = new Vector3(pos.x + 45, pos.y, pos.z);
                        break;
                }
                globalItemShape.SetBlockPos();
                AudioController.Instance.PlaySound("MoveX");
            }
        }

        private void EventDropFastest()
        {
            if (!isFastDrop)
            {
                return;
            }
            isFastDrop = false;
            fDropInterval = fDropIntervalFastest;
            StopCoroutine(_mIEBlockDrop);
            StartCoroutine(_mIEBlockDrop);
        }

        private void EventShapeRotate(ShapeChange rotateDirection)
        {
            int nextShapeIndex = 0;
            switch (rotateDirection)
            {
                case ShapeChange.RotateA:
                    nextShapeIndex = globalItemShape.shapeIndex - 1;
                    break;
                case ShapeChange.RotateB:
                    nextShapeIndex = globalItemShape.shapeIndex + 1;
                    break;
            }

            if (globalItemShape.JudgeIsPossibleRotate(panelAllBlock, nextShapeIndex))
            {
                globalItemShape.shapeIndex = nextShapeIndex;
                globalItemShape.SetBlockPos();
                AudioController.Instance.PlaySound("Rotate");
            }
        }
        #endregion
    }
}