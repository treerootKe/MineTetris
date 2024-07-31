using System;
using Tetris.Control;

namespace Tetris.Manage
{
    public class TetrisEventManager
    {
        public static Action<ShapeChange> eventShapeMoveX;          //��״ˮƽ�ƶ�
        public static Action<ShapeChange> eventShapeRotate;         //��״��ת
        public static Action eventDropFastest;                      //��״��������
        public static Action<int> eventChangeLevel;                 //������Ϸ�Ѷ�(��״�����ٶ�)

        public static Action eventPauseGame;                        //��ͣ��Ϸ
        public static Action eventStartGame;                        //��ʼ��Ϸ
        public static Action eventRestartGame;                      //���¿�ʼ��Ϸ
    }
}