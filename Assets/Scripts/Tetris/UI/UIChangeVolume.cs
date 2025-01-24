using Manage;
using Manage.LoadAssetsManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.UI
{
    class UIChangeVolume:MonoBehaviour
    {
        private Slider _sliderSoundEffect;
        private Slider _sliderSoundBgm;
        private Button _btnClose;
        
        private void Awake()
        {
            FindComponent();
            _sliderSoundEffect.onValueChanged.AddListener(value =>
            {
                EventManager.EventChangeSoundEffectValue?.Invoke(value);
            });
            _sliderSoundBgm.onValueChanged.AddListener(value =>
            {
                EventManager.EventChangeSoundBgmValue?.Invoke(value);
            });
            _btnClose.onClick.AddListener(() => LoadManager<GameObject>.ClosePrefab(AssetsName.ChangeVolume));
        }

        private void FindComponent()
        {
            _sliderSoundEffect = transform.Find("txtSoundEffect/sliderSoundEffect").GetComponent<Slider>();
            _sliderSoundBgm = transform.Find("txtSoundBackground/sliderSoundBackground").GetComponent<Slider>();
            _btnClose = transform.Find("btnClose").GetComponent<Button>();
        }
    }
}
