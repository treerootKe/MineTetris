using System;
using Common;
using Manage.LoadAssetsManage;
using UnityEngine;
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
                PlayerData.gamesName = GamesName.Tetris;
                StartCoroutine(LoadManager<GameObject>.ShowPrefab(AssetsName.MainTetris));
                this.gameObject.SetActive(false);
            });
            btnEnterTetris1.onClick.AddListener(() =>
            {
                PlayerData.gamesName = GamesName.Tetris;
                StartCoroutine(LoadManager<GameObject>.ShowPrefab(AssetsName.MainTetris));
                this.gameObject.SetActive(false);
            });
        }
    }
}