using System;
using Tetris.Control;

namespace Tetris.Manage
{
    public class EventManager
    {
        public static Action<ShapeChange> eventShapeMoveX;          //��״ˮƽ�ƶ�
        public static Action<ShapeChange> eventShapeRotate;         //��״��ת
        public static Action eventDropFastest;                      //��״��������
    }
}