using Cinemachine;
using DG.Tweening;
using HollowKnight;
using UnityEngine;

namespace Common
{
    public class CommonMethod: MonoBehaviour
    {
        /// <summary>
        /// 游戏物体抖动
        /// </summary>
        /// <param name="traObject">需要抖动的游戏物体</param>
        /// <param name="time">抖动时间</param>
        /// <param name="strength">抖动强度</param>
        /// <param name="vibrato">抖动次数</param>
        public static void ObjectShake(Transform traObject, float time, Vector3 strength, int vibrato)
        {
            var initialLocalPosition = traObject.localPosition;
            traObject.DOShakePosition(time, strength, vibrato).OnComplete(() =>
            {
                traObject.transform.localPosition = initialLocalPosition;
            });
        }
        
        /// <summary>
        /// 相机抖动
        /// </summary>
        /// <param name="time">抖动时间</param>
        public static void CameraShake(float time)
        {
            CommonMembers.CinemaChineBasicMulti.m_AmplitudeGain = 1;
            DOVirtual.DelayedCall(time, () =>
            {
                CommonMembers.CinemaChineBasicMulti.m_AmplitudeGain = 0;
            });
        }
        
        /// <summary>
        /// 本地轨迹移动
        /// </summary>
        /// <param name="initTransform">移动的游戏物体</param>
        /// <param name="initPos">初始位置</param>
        /// <param name="endPos">结束位置</param>
        /// <param name="time">移动时间</param>
        /// <param name="strength">强度</param>
        /// <param name="type">缓动函数类型</param>
        public static void DoLocalPath(Transform initTransform, Vector3 initPos, Vector3 endPos, float time, float strength = 1,Ease type = Ease.Linear)
        {
            Vector3 dir = endPos - initPos;
            Vector3 verticalInitEnd = Vector3.Cross(initPos, endPos).normalized;
        
            //如果叉积为0，说明两个向量共线
            if (verticalInitEnd == Vector3.zero)
            {
                verticalInitEnd = Vector3.Cross(dir, Vector3.down).normalized;
                if (dir.x == 0 && dir.z == 0)
                {
                    verticalInitEnd = dir.y > 0 ? Vector3.back : Vector3.forward;
                }
            }
            Vector3 verticalDir = Vector3.Cross(dir, verticalInitEnd).normalized;
            Vector3 midInitEnd = (initPos + endPos) / 2f;
            Vector3 upInitEnd = midInitEnd + verticalDir * strength;

            Vector3[] vec3List = new Vector3[] {initPos, upInitEnd, endPos };
            initTransform.DOLocalPath(vec3List, time).SetEase(type).OnComplete(() => initTransform.gameObject.SetActive(false));
        }
        
        /// <summary>
        /// 轨迹移动
        /// </summary>
        /// <param name="initTransform">移动的游戏物体</param>
        /// <param name="initPos">初始位置</param>
        /// <param name="endPos">结束位置</param>
        /// <param name="time">移动时间</param>
        /// <param name="strength">强度</param>
        /// <param name="type">缓动函数类型</param>
        public static void DoPath(Transform initTransform, Vector3 initPos, Vector3 endPos, float time, float strength = 1,Ease type = Ease.Linear)
        {
            Vector3 dir = endPos - initPos;
            Vector3 verticalInitEnd = Vector3.Cross(initPos, endPos).normalized;
        
            //如果叉积为0，说明两个向量共线
            if (verticalInitEnd == Vector3.zero)
            {
                verticalInitEnd = Vector3.Cross(dir, Vector3.down).normalized;
                if (dir.x == 0 && dir.z == 0)
                {
                    verticalInitEnd = dir.y > 0 ? Vector3.back : Vector3.forward;
                }
            }
            Vector3 verticalDir = Vector3.Cross(dir, verticalInitEnd).normalized;
            Vector3 midInitEnd = (initPos + endPos) / 2f;
            Vector3 upInitEnd = midInitEnd + verticalDir * strength;
            Vector3[] vec3List = new Vector3[] { initPos, upInitEnd, endPos };
            initTransform.DOLocalPath(vec3List, time).SetEase(type).OnComplete(() => initTransform.gameObject.SetActive(false));
        }
    }
}