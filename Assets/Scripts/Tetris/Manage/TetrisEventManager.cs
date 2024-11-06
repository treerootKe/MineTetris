using System;
using Tetris.Control;

namespace Tetris.Manage
{
    public class TetrisEventManager
    {
        public static Action<ShapeChange> eventShapeMoveX;          //????????
        public static Action<ShapeChange> eventShapeRotate;         //??????
        public static Action eventDropFastest;                      //???????????
        public static Action<int> eventChangeLevel;                 //??????????(??????????)

        public static Action eventPauseGame;                        //??????
        public static Action eventStartGame;                        //??????
        public static Action eventRestartGame;                      //?????????
    }
}