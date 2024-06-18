using System;
using System.Collections.Generic;
using DG.Tweening;
using Tetris.Common;
using Tetris.DesignPattern;
using Tetris.ToolClasses;
using UnityEngine;

namespace Tetris.Control
{
    public class AudioController : MonoSingleton<AudioController>
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
            TetrisCommonMembers.audioPool = new ObjectPool<AudioSource>(audioSource);
        }

        public void PlaySound(string clipName)
        {
            var sourceAudio = TetrisCommonMembers.audioPool.Get();
            sourceAudio.clip = _dicAudioClips[clipName];
            sourceAudio.Play();
            DOVirtual.DelayedCall(sourceAudio.clip.length, () => TetrisCommonMembers.audioPool.Recycle(sourceAudio));
        }
    }
}