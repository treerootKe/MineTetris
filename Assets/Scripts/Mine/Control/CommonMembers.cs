using Mine.ObjectPoolItem;
using Mine.ToolClasses;
using System.Collections.Generic;
using UnityEngine;

namespace Mine.Control
{
    public class CommonMembers
    {
        public static ObjectPool<AudioSource> audioPool;
        public static ObjectPool<ItemShape>[] shapePool;
        public static ObjectPool<Transform> blockPool;
        public static List<List<Vector2[]>> blockRotateInsidePos;//形状旋转后内部位置集合 
        
        
        public static void InitValue()
        {
            blockRotateInsidePos = new List<List<Vector2[]>>();
            //I
            var shapeArea = new List<Vector2[]>
            {
                new Vector2[4] { new Vector2(0, 90), new Vector2(45, 90), new Vector2(90, 90), new Vector2(135, 90) },
                new Vector2[4] { new Vector2(90, 0), new Vector2(90, 45), new Vector2(90, 90), new Vector2(90, 135) },
                new Vector2[4] { new Vector2(0, 45), new Vector2(45, 45), new Vector2(90, 45), new Vector2(135, 45) },
                new Vector2[4] { new Vector2(45, 0), new Vector2(45, 45), new Vector2(45, 90), new Vector2(45, 135) }
            };
            blockRotateInsidePos.Add(shapeArea);
            //T
            var oneShapeArea = new List<Vector2[]>
            {
                new Vector2[4] { new Vector2(45, 45), new Vector2(0, 90), new Vector2(45, 90), new Vector2(90, 90) },
                new Vector2[4] { new Vector2(90, 0), new Vector2(45, 45), new Vector2(90, 45), new Vector2(90, 90) },
                new Vector2[4] { new Vector2(0, 0), new Vector2(45, 0), new Vector2(90, 0), new Vector2(45, 45) },
                new Vector2[4] { new Vector2(0, 0), new Vector2(0, 45), new Vector2(45, 45), new Vector2(0, 90) }
            };
            blockRotateInsidePos.Add(oneShapeArea);
            //L
            var twoShapeArea = new List<Vector2[]>
            {
                new Vector2[4] { new Vector2(0, 45), new Vector2(0, 90), new Vector2(45, 90), new Vector2(90, 90) },
                new Vector2[4] { new Vector2(90, 0), new Vector2(90, 45), new Vector2(45, 90), new Vector2(90, 90) },
                new Vector2[4] { new Vector2(0, 0), new Vector2(45, 0), new Vector2(90, 0), new Vector2(90, 45) },
                new Vector2[4] { new Vector2(0, 0), new Vector2(45, 0), new Vector2(0, 45), new Vector2(0, 90) }
            };
            blockRotateInsidePos.Add(twoShapeArea);
            //J
            var threeShapeArea = new List<Vector2[]>
            {
                new Vector2[4] { new Vector2(90, 45), new Vector2(0, 90), new Vector2(45, 90), new Vector2(90, 90) },
                new Vector2[4] { new Vector2(45, 0), new Vector2(90, 0), new Vector2(90, 45), new Vector2(90, 90) },
                new Vector2[4] { new Vector2(0, 0), new Vector2(45, 0), new Vector2(90, 0), new Vector2(0, 45) },
                new Vector2[4] { new Vector2(0, 0), new Vector2(0, 45), new Vector2(0, 90), new Vector2(45, 90) }
            };
            blockRotateInsidePos.Add(threeShapeArea);
            //S
            var fourShapeArea = new List<Vector2[]>
            {
                new Vector2[4] { new Vector2(0, 45), new Vector2(45, 45), new Vector2(45, 90), new Vector2(90, 90) },
                new Vector2[4] { new Vector2(90, 0), new Vector2(45, 45), new Vector2(90, 45), new Vector2(45, 90) },
                new Vector2[4] { new Vector2(0, 0), new Vector2(45, 0), new Vector2(45, 45), new Vector2(90, 45) },
                new Vector2[4] { new Vector2(45, 0), new Vector2(0, 45), new Vector2(45, 45), new Vector2(0, 90) }
            };
            blockRotateInsidePos.Add(fourShapeArea);
            //Z
            var fiveShapeArea = new List<Vector2[]>
            {
                new Vector2[4] { new Vector2(45, 45), new Vector2(90, 45), new Vector2(0, 90), new Vector2(45, 90) },
                new Vector2[4] { new Vector2(45, 0), new Vector2(45, 45), new Vector2(90, 45), new Vector2(90, 90) },
                new Vector2[4] { new Vector2(45, 0), new Vector2(90, 0), new Vector2(0, 45), new Vector2(45, 45) },
                new Vector2[4] { new Vector2(0, 0), new Vector2(0, 45), new Vector2(45, 45), new Vector2(45, 90) }
            };
            blockRotateInsidePos.Add(fiveShapeArea);
            //O
            var sixShapeArea = new List<Vector2[]>
            {
                new Vector2[4] { new Vector2(0 , 0), new Vector2(45, 0), new Vector2(0, 45), new Vector2(45, 45) },
                new Vector2[4] { new Vector2(0 , 0), new Vector2(45, 0), new Vector2(0, 45), new Vector2(45, 45)  },
                new Vector2[4] { new Vector2(0 , 0), new Vector2(45, 0), new Vector2(0, 45), new Vector2(45, 45)  },
                new Vector2[4] { new Vector2(0 , 0), new Vector2(45, 0), new Vector2(0, 45), new Vector2(45, 45) }
            };
            blockRotateInsidePos.Add(sixShapeArea);
        }
    }
}
