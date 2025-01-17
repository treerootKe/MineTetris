using System;
using Cinemachine;
using UnityEngine;

namespace HollowKnight
{
    public class CommonMembers
    {
        private static CinemachineBasicMultiChannelPerlin _playerCamera;

        public static CinemachineBasicMultiChannelPerlin PlayerCamera
        {
            get
            {
                if (_playerCamera == null)
                {
                    _playerCamera = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                }
                return _playerCamera;
            }
        }
        
    }
}