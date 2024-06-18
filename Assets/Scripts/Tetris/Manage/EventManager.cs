using System;
using Tetris.Control;

namespace Tetris.Manage
{
    public class EventManager
    {
        public static Action<ShapeChange> eventShapeMoveX;          //形状水平移动
        public static Action<ShapeChange> eventShapeRotate;         //形状旋转
        public static Action eventDropFastest;                      //形状快速下落
    }
}