using Manage;
using Manage.UIManage;
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
        private Slider sliderSoundEffect;
        private Slider sliderSoundBgm;
        private Button btnClose;
        
        private void Awake()
        {
            FindComponent();
            sliderSoundEffect.onValueChanged.AddListener(value =>
            {
                EventManager.eventChangeSoundEffectValue?.Invoke(value);
            });
            sliderSoundBgm.onValueChanged.AddListener(value =>
            {
                EventManager.eventChangeSoundBgmValue?.Invoke(value);
            });
            btnClose.onClick.AddListener(() => UIManager.CloseUI(UIPath.UIChangeVolume));
        }

        private void FindComponent()
        {
            sliderSoundEffect = transform.Find("txtSoundEffect/sliderSoundEffect").GetComponent<Slider>();
            sliderSoundBgm = transform.Find("txtSoundBackground/sliderSoundBackground").GetComponent<Slider>();
            btnClose = transform.Find("btnClose").GetComponent<Button>();
        }
    }
}
