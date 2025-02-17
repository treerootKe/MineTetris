using System;
using Cinemachine;
using UnityEngine;

namespace HollowKnight
{
    public class CommonMembers
    {
        private static CinemachineBasicMultiChannelPerlin s_cinemaChineBasicMulti;

        public static CinemachineBasicMultiChannelPerlin CinemaChineBasicMulti
        {
            get
            {
                if (s_cinemaChineBasicMulti == null)
                {
                    s_cinemaChineBasicMulti = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                }
                return s_cinemaChineBasicMulti;
            }
        }
        
    }
}