using System;
using Tetris.Control;

namespace Tetris.Manage
{
    public class TetrisEventManager
    {
        public static Action<ShapeChange> eventShapeMoveX;          //形状水平移动
        public static Action<ShapeChange> eventShapeRotate;         //形状旋转
        public static Action eventDropFastest;                      //形状快速下落
        public static Action<int> eventChangeLevel;                 //更改游戏难度(形状下落速度)

        public static Action eventPauseGame;                        //暂停游戏
        public static Action eventStartGame;                        //开始游戏
        public static Action eventRestartGame;                      //重新开始游戏
    }
}