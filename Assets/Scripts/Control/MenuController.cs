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
        private Button btnEnterTetris;
        private Button btnEnterTetris1;
        private void Awake()
        {
            FindComponents();
            RegisterButtonEvent();
        }

        private void FindComponents()
        {
            btnEnterTetris = transform.Find("GameList/btnEnterTetris").GetComponent<Button>();
            btnEnterTetris1= transform.Find("GameList/btnEnterTetris1").GetComponent<Button>();
        }

        private void RegisterButtonEvent()
        {
            btnEnterTetris.onClick.AddListener(() =>
            {
                PlayerData.GamesName = GamesName.Tetris;
                StartCoroutine(LoadManager<GameObject>.ShowPrefab(AssetsName.MainTetris));
                gameObject.SetActive(false);
            });
            btnEnterTetris1.onClick.AddListener(() =>
            {
                PlayerData.GamesName = GamesName.Tetris;
                StartCoroutine(LoadManager<GameObject>.ShowPrefab(AssetsName.MainTetris));
                gameObject.SetActive(false);
            });
        }
    }
}