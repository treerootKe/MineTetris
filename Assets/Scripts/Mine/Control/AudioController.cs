using System;
using System.Collections.Generic;
using DG.Tweening;
using Mine.DesignPattern;
using Mine.ToolClasses;
using UnityEngine;

namespace Mine.Control
{
    public class AudioController : Singleton<AudioController>
    {
        private Dictionary<string, AudioClip> _dicAudioClips;
        public AudioClip audioClipMoveX;
        public AudioClip audioClipDrop;
        public AudioClip audioClipRotate;
        public AudioClip audioClipDisappear;

        public AudioSource audioSource;
        public AudioSource audioSourceSingle;
        public AudioSource audioSourceBgm;
        

        protected override void Awake()
        {
            base.Awake();
            _dicAudioClips = new Dictionary<string, AudioClip>
            {
                { "MoveX", audioClipMoveX },
                { "Drop", audioClipDrop },
                { "Rotate", audioClipRotate },
                { "Disappear", audioClipDisappear }
            };
            CommonMembers.audioPool = new ObjectPool<AudioSource>(audioSource);
        }

        public void PlaySound(string clipName)
        {
            var sourceAudio = CommonMembers.audioPool.Get();
            sourceAudio.clip = _dicAudioClips[clipName];
            sourceAudio.Play();
            DOVirtual.DelayedCall(sourceAudio.clip.length, () => CommonMembers.audioPool.Recycle(sourceAudio));
        }
    }
}