using System;
using System.Linq;
using Common;
using Manage.LoadAssetsManage;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

namespace Control
{
    public class MenuController:MonoBehaviour
    {
        private Button _btnEnterTetris;
        private Button _btnEnterTetris1;
        private void Awake()
        {
            FindComponents();
            RegisterButtonEvent();
        }

        private void FindComponents()
        {
            _btnEnterTetris = transform.Find("GameList/btnEnterTetris").GetComponent<Button>();
            _btnEnterTetris1= transform.Find("GameList/btnEnterTetris1").GetComponent<Button>();
        }

        private void RegisterButtonEvent()
        {
            _btnEnterTetris.onClick.AddListener(() =>
            {
                PlayerData.S_GamesName = GamesName.Tetris;
                StartCoroutine(LoadManager<GameObject>.ShowPrefab(AssetsName.MainTetris));
                gameObject.SetActive(false);
            });
            _btnEnterTetris1.onClick.AddListener(() =>
            {
                PlayerData.S_GamesName = GamesName.Tetris;
                StartCoroutine(LoadManager<GameObject>.ShowPrefab(AssetsName.MainTetris));
                gameObject.SetActive(false);
            });
        }
    }
}