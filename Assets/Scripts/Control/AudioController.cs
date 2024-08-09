using System;
using System.Collections.Generic;
using DG.Tweening;
using Tetris.Common;
using DesignPattern;
using ToolClasses;
using UnityEngine;
using Manage;

namespace Control
{
    public class AudioController : MonoSingleton<AudioController>
    {
        private List<AudioSource> _listAudioSources;
        private Dictionary<string, AudioClip> _dicAudioClips;
        public AudioClip audioClipMoveX;
        public AudioClip audioClipDrop;
        public AudioClip audioClipRotate;
        public AudioClip audioClipDisappear;
        public AudioClip audioBgm;

        public AudioSource audioSource;
        public AudioSource audioSourceSingle;
        public AudioSource audioSourceBgm;

        private float _valueSoundEffect;
        private float _valueSoundBgm;

        protected override void Awake()
        {
            base.Awake();
            _listAudioSources = new List<AudioSource>();
            _dicAudioClips = new Dictionary<string, AudioClip>
            {
                { "MoveX", audioClipMoveX },
                { "Drop", audioClipDrop },
                { "Rotate", audioClipRotate },
                { "Disappear", audioClipDisappear },
                { "Bgm", audioBgm }
            };
            TetrisCommonMembers.audioPool = new ObjectPool<AudioSource>(audioSource);
        }

        private void OnEnable()
        {
            EventManager.eventChangeSoundEffectValue += EventChangeSoundEffectValue;
            EventManager.eventChangeSoundBgmValue += EventChangeBgmValue;
        }

        private void OnDisable()
        {
            EventManager.eventChangeSoundEffectValue -= EventChangeSoundEffectValue;
            EventManager.eventChangeSoundBgmValue -= EventChangeBgmValue;
        }

        public void PlaySound(string clipName)
        {
            var sourceAudio = TetrisCommonMembers.audioPool.Get();
            _listAudioSources.Add(sourceAudio);
            sourceAudio.clip = _dicAudioClips[clipName];
            sourceAudio.volume = _valueSoundEffect;
            sourceAudio.Play();
            DOVirtual.DelayedCall(sourceAudio.clip.length, () => TetrisCommonMembers.audioPool.Recycle(sourceAudio));
        }

        public void PlaySingleSound(string clipName)
        {
            audioSourceSingle.clip = _dicAudioClips[clipName];
            audioSourceSingle.volume = _valueSoundEffect;
            audioSourceSingle.Play();
        }

        public void PlayBgm(string clipName)
        {
            audioSourceBgm.clip = _dicAudioClips[clipName];
            audioSourceBgm.volume = _valueSoundBgm;
            audioSourceBgm.Play();
        }


        private void EventChangeSoundEffectValue(float value)
        {
            _valueSoundEffect = value;
            foreach (var item in _listAudioSources)
            {
                if (item.gameObject.activeSelf)
                {
                    item.volume = value;
                }
            }
        }
        private void EventChangeBgmValue(float value)
        {
            _valueSoundBgm = value;
            audioSourceBgm.volume = value;
        }
    }
}