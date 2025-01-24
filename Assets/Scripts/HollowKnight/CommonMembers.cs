using System;
using Cinemachine;
using UnityEngine;

namespace HollowKnight
{
    public class CommonMembers
    {
        private static CinemachineBasicMultiChannelPerlin _cinemaChineBasicMulti;

        public static CinemachineBasicMultiChannelPerlin CinemaChineBasicMulti
        {
            get
            {
                if (_cinemaChineBasicMulti == null)
                {
                    _cinemaChineBasicMulti = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                }
                return _cinemaChineBasicMulti;
            }
        }
        
    }
}