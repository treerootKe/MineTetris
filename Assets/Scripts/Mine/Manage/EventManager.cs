using System;
using Mine.Control;

namespace Mine.Manage
{
    public class EventManager
    {
        public static Action<ShapeChange> eventShapeMoveX;
        public static Action<ShapeChange> eventShapeRotate;
        public static Action eventDropFastest;
    }
}