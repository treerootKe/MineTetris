using System.Collections.Generic;
using Tetris.Common;
using UnityEngine;
using Tetris.Control;

namespace Tetris.ObjectPoolItem
{
    public class ItemShape: MonoBehaviour
    {
        public int shapeType;                           //形状的类型
        public int shapeIndex;                          //形状当前变换的位置
        public int[] blockPos;                          //方块在整个下落区域的位置集合
        public Transform[] fourBlock;                   //形状内部的四个方块
        private List<Vector2[]> _mBlockRotateInsidePos; //形状改变后，方块在内部位置坐标集合
        private void Awake()
        {
            blockPos = new int[4];
            fourBlock = new Transform[4];
            _mBlockRotateInsidePos = TetrisCommonMembers.blockRotateInsidePos[shapeType];
        }
        private void OnEnable()
        {
            transform.localPosition = shapeType == 6 ? new Vector2(180, 810) : new Vector2(135, 765);
            for (int i = 0; i < fourBlock.Length; i++)
            {
                fourBlock[i] = TetrisCommonMembers.blockPool.Get(transform);
            }
            shapeIndex = 0;
            SetBlockPos();
        }

        //设置4个方块在整个下落区域的坐标
        public void SetBlockPos()
        {
            if (shapeIndex < 0)
            {
                shapeIndex = 3;
            }
            if (shapeIndex > 3)
            {
                shapeIndex = 0;
            }
            var pos = transform.localPosition;
            var posTrans = (int)(pos.y * 10 + pos.x) / 45;
            for (int i = 0; i < fourBlock.Length; i++)
            {
                var posBlock = (int)(_mBlockRotateInsidePos[shapeIndex][i].y * 10 + _mBlockRotateInsidePos[shapeIndex][i].x) / 45;
                fourBlock[i].localPosition = _mBlockRotateInsidePos[shapeIndex][i];
                blockPos[i] = posTrans + posBlock;
            }
        }

        //判断是否可以旋转
        public bool JudgeIsPossibleRotate(List<Transform> allPos,int nextShapeIndex)
        {
            var pos = transform.localPosition;
            var posTrans = (int)(pos.y * 10 + pos.x) / 45;
            var nextBlockPos = new int[4];
            if (nextShapeIndex < 0)
            {
                nextShapeIndex = 3;
            }
            else if (nextShapeIndex > 3)
            {
                nextShapeIndex = 0;
            }
            for (int i = 0; i < 4; i++)
            {
                var posX = pos.x + _mBlockRotateInsidePos[nextShapeIndex][i].x;
                var posY = pos.y + _mBlockRotateInsidePos[nextShapeIndex][i].y;
                var posBlock = (int)(_mBlockRotateInsidePos[nextShapeIndex][i].y * 10 + _mBlockRotateInsidePos[nextShapeIndex][i].x) / 45;
                nextBlockPos[i] = posTrans + posBlock;
                if (posX > 405 || posX < 0  || posY < 0 || allPos[nextBlockPos[i]] != null)
                {
                    return false;
                }
            }
            return true;
        }
        
        //判断是否可以水平移动
        public bool JudgeIsPossibleMoveX(List<Transform> allPos,ShapeChange direction)
        {
            var nextBlockPos = new int[4];
            for (int i = 0; i < 4; i++)
            {
                if (blockPos[i] % 10 == 0 && direction == ShapeChange.Left)
                {
                    return false;
                }
                if (blockPos[i] % 10 == 9 && direction == ShapeChange.Right)
                {
                    return false;
                }
                nextBlockPos[i] = direction == ShapeChange.Left ? blockPos[i] - 1 : blockPos[i] + 1;
                if (allPos[nextBlockPos[i]] != null)
                {
                    return false;
                }
            }
            return true;
        }
        
        //判断是否能继续下落
        public bool JudgeIsPossibleDrop(List<Transform> allPos)
        {
            var nextBlockPos = new int[4];
            for (int i = 0; i < 4; i++)
            {
                if (blockPos[i] < 10)
                {
                    return false;
                }
                nextBlockPos[i] = blockPos[i] - 10;
                if (allPos[nextBlockPos[i]] != null)
                {
                    return false;
                }
            }
            return true;
        }

        //回收这一个Shape
        public bool RecycleShape()
        {
            int count = 0;
            foreach (var item in fourBlock)
            {
                if (!item.gameObject.activeInHierarchy)
                {
                    count++;
                }
            }

            if (count == 4)
            {
                TetrisCommonMembers.shapePool[shapeType].Recycle(transform.GetComponent<ItemShape>());
                return true;
            }

            return false;
        }
    }
}
