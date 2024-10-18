using System.Collections;
using UnityEngine;
using Tetris.ObjectPoolItem;
using System.Collections.Generic;
using Common;
using DG.Tweening;
using ToolClasses;
using Tetris.Common;
using DesignPattern;
using UnityEngine.UI;
using Tetris.Manage;
using Control;

namespace Tetris.Control
{
    public enum ShapeChange
    {
        Left,
        Right,
        RotateA,
        RotateB
    }

    public  class MainTetrisControl : MonoSingleton<MainTetrisControl>
    {
        public Transform traTopPanel;//顶部面板
        public Transform traBottomPanel;//底部面板
        public Transform traDirectionKeys;//方向键面板

        public Transform traDropPanel; //下落区域的父物体
        public Transform traPrefab; //预制体的父物体
        public List<Transform> panelAllBlock; //下落区域中的200个方块
        public List<ItemShape> panelAllShape; //下落区域中，所有的形状
        public GameObject[] gameObjectsNextShape; //下一个形状的游戏物体数组
        public ItemShape globalItemShape; //当前正在下落的形状

        private int _mScore; //当前总得分
        private bool isFastDrop; //形状是否可以进行快速下落                  
        private int _mNextShape; //下一个形状的索引(0--6，分别对应7种形状)
        private float fDropInterval; //形状当前下落间隔
        private float[] fDropIntervals; //形状每下落一次，需要等待间隔的数组
        private int nDropIntervalLevel; //形状下落间隔等级(0--2)
        private float fDropIntervalFastest; //形状快速下落时的等待间隔
        public bool isPausing; //是否正在暂停
        public bool isGameOver; //是否游戏结束
        private List<int> disappearRow = new List<int>();//本轮消失行的列表

        public Text txtScore; //分数显示文本
        private Text txtHistoryScore; //历史分数显示文本
        private Text txtLevel; //形状下落间隔等级显示文本

        private IEnumerator _mIEBlockDrop; //形状下落的协程

        protected override void Awake()
        {
            AudioController.Instance.PlayBgm("Bgm");
            base.Awake();
            FindComponent();
            InitValues();
        }

        private void InitValues()
        {
            PlayerData.gamesName = GamesName.Tetris;
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
                TetrisCommonMembers.shapePool[i] =
                    new ObjectPool<ItemShape>(traPrefab.GetChild(i).GetComponent<ItemShape>());
            }

            TetrisCommonMembers.blockPool = new ObjectPool<Transform>(traPrefab.Find("block"));
            fDropIntervals = new float[3] { 1, 0.5f, 0.25f };
            fDropIntervalFastest = 0.05f;
            fDropInterval = fDropIntervals[nDropIntervalLevel];
            _mNextShape = Random.Range(0, 7);
        }

        //获取组件
        private void FindComponent()
        {
            traTopPanel = transform.Find("TopPanel");
            traBottomPanel = transform.Find("BottomPanel");
            traDirectionKeys = transform.Find("DirectionKeys");

            traDropPanel = transform.Find("BlockDropArea/DropPanel");
            traPrefab = transform.Find("Prefab");
            txtScore = transform.Find("ScoreArea/imgScore/txtScore").GetComponent<Text>();
            txtHistoryScore = transform.Find("ScoreArea/imgHistoryScore/txtHistoryScore").GetComponent<Text>();
            txtLevel = transform.Find("ScoreArea/imgLevel/txtLevel").GetComponent<Text>();
            gameObjectsNextShape = new GameObject[7];
            for (int i = 0; i < 7; i++)
            {
                gameObjectsNextShape[i] = transform.Find("ScoreArea/imgNextShape").GetChild(i).gameObject;
            }
        }

        private void OnEnable()
        {
            TetrisEventManager.eventShapeMoveX += EventShapeMoveX;
            TetrisEventManager.eventDropFastest += EventDropFastest;
            TetrisEventManager.eventShapeRotate += EventShapeRotate;
            TetrisEventManager.eventChangeLevel += EventChangeLevel;

            TetrisEventManager.eventPauseGame += EventPauseGame;
            TetrisEventManager.eventStartGame += EventStartGame;
            TetrisEventManager.eventRestartGame += EventRestartGame;
        }

        private void OnDisable()
        {
            TetrisEventManager.eventShapeMoveX -= EventShapeMoveX;
            TetrisEventManager.eventDropFastest -= EventDropFastest;
            TetrisEventManager.eventShapeRotate -= EventShapeRotate;
            TetrisEventManager.eventChangeLevel -= EventChangeLevel;

            TetrisEventManager.eventPauseGame -= EventPauseGame;
            TetrisEventManager.eventStartGame -= EventStartGame;
            TetrisEventManager.eventRestartGame -= EventRestartGame;
        }

        private void Update()
        {
            if (!isGameOver && globalItemShape != null)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    TetrisEventManager.eventShapeRotate?.Invoke(ShapeChange.RotateA);
                }

                if (Input.GetKeyDown(KeyCode.B))
                {
                    TetrisEventManager.eventShapeRotate?.Invoke(ShapeChange.RotateB);
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    TetrisEventManager.eventShapeMoveX?.Invoke(ShapeChange.Left);
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    TetrisEventManager.eventShapeMoveX?.Invoke(ShapeChange.Right);
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    TetrisEventManager.eventDropFastest?.Invoke();
                }
            }
        }

        //生成一个新的形状前，初始化一下数据
        public void OnceDropInit()
        {
            isFastDrop = true;
            fDropInterval = fDropIntervals[nDropIntervalLevel];
            gameObjectsNextShape[_mNextShape].SetActive(false);
            globalItemShape = TetrisCommonMembers.shapePool[_mNextShape].Get(traDropPanel);
            panelAllShape.Add(globalItemShape);
            _mNextShape = Random.Range(0, 7);
            gameObjectsNextShape[_mNextShape].SetActive(true);
            _mIEBlockDrop = ShapeDrop(globalItemShape);
            StartTetris();
        }

        //开始判定
        private void StartTetris()
        {
            if (!globalItemShape.JudgeIsPossibleDrop(panelAllBlock)) 
            {
                isGameOver = true;
                TetrisEventManager.eventPauseGame?.Invoke();
                return;
            }

            StartCoroutine(_mIEBlockDrop);
        }

        //形状下落
        IEnumerator ShapeDrop(ItemShape item, bool isRecursion = false)
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
                yield return ShapeDrop(item, true);
            }

            if (isRecursion)
            {
                yield break;
            }

            for (int i = 0; i < 4; i++)
            {
                panelAllBlock[item.posFourBlock[i]] = item.traFourBlock[i];
            }

            globalItemShape = null;
            yield return BlockDisappear();
            while (isPausing)
            {
                yield return null;
            }

            OnceDropInit();
        }

        //方块消失
        IEnumerator BlockDisappear()
        {
            disappearRow.Clear();
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
                for (int i = disappearRow[0] + 10; i < 200; i += 10)
                {
                    var dropCounts = RowDropCounts(disappearRow, i);
                    if (i >= 10 && JudgeIsPossibleDrop(i) && dropCounts != 0)
                    {
                        for (int j = i; j < i + 10; j++)
                        {
                            if (panelAllBlock[j] != null)
                            {
                                panelAllBlock[j].DOLocalMoveY(panelAllBlock[j].transform.localPosition.y - 45 * dropCounts, 0.01f);
                                panelAllBlock[j - dropCounts * 10] = panelAllBlock[j];
                                panelAllBlock[j] = null;
                            }
                        }
                    }
                }

                yield return new WaitForSeconds(0.01f);

                for (int i = 0; i < panelAllShape.Count; i++)
                {
                    if (panelAllShape[i].IsPossibleRecycle())
                    {
                        panelAllShape.RemoveAt(i);
                    }
                }

                AudioController.Instance.PlaySound("Disappear");
                yield return new WaitForSeconds(1f);
            }
        }

        //加分，数字滚动
        private void AddScore(int nDisappearLine)
        {
            int nOnceScore = nDisappearLine * nDisappearLine * 100;
            DOTween.To(endValue => 
            { 
                txtScore.text = ((int)endValue).ToString(); 
            }, _mScore, _mScore + nOnceScore, 1).OnComplete(() => _mScore += nOnceScore);
        }

        //判断该行是否能够下落
        private bool JudgeIsPossibleDrop(int rowIndex)
        {
            for (int i = rowIndex; i < rowIndex + 10; i++)
            {
                if (panelAllBlock[i] != null)
                {
                    return true;
                }
            }

            return false;
        }

        //一行方块下落的格数
        private int RowDropCounts(List<int> disappearRow, int rowIndex)
        {
            int count = 0;
            foreach (var t in disappearRow)
            {
                if (rowIndex > t)
                {
                    count++;
                }
            }

            return count;
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

        //改变形状下落速度
        private void EventChangeLevel(int index)
        {
            nDropIntervalLevel = index;
            txtLevel.text = (index + 1).ToString();
            fDropInterval = fDropIntervals[index];
        }


        public void EventPauseGame()
        {
            isPausing = true;
            traTopPanel.gameObject.SetActive(false);
            traDirectionKeys.gameObject.SetActive(false);
            traBottomPanel.gameObject.SetActive(true);
        }

        public void EventStartGame()
        {
            isPausing = false;
            traTopPanel.gameObject.SetActive(true);
            traDirectionKeys.gameObject.SetActive(true);
            traBottomPanel.gameObject.SetActive(false);
            if (globalItemShape == null)
            {
                OnceDropInit();
            }
            else if (isGameOver)
            {
                TetrisEventManager.eventRestartGame?.Invoke();
            }
        }
        public void EventRestartGame()
        {
            StopCoroutine(_mIEBlockDrop);
            globalItemShape.RecycleBlock();
            for (int i = 0; i < panelAllBlock.Count; i++)
            {
                if (panelAllBlock[i] != null)
                {
                    TetrisCommonMembers.blockPool.Recycle(panelAllBlock[i]);
                    panelAllBlock[i] = null;
                }
            }
            foreach (var item in panelAllShape)
            {
                TetrisCommonMembers.shapePool[item.shapeType].Recycle(item);
            }
            isPausing = false;
            traTopPanel.gameObject.SetActive(true);
            traDirectionKeys.gameObject.SetActive(true);
            traBottomPanel.gameObject.SetActive(false);
            txtScore.text = "0";
            isGameOver = false;
            panelAllShape.Clear();
            OnceDropInit();
        }
    }
    #endregion
}