
using System;
using UnityEngine;

namespace Tetris.DesignPattern
{
    public class MonoSingleton<T>: MonoBehaviour where T:MonoBehaviour
    {
        private static T _instance;

        public static T Instance => _instance;
        protected virtual void Awake()
        {
            _instance = transform.GetComponent<T>();
            Debug.Log(_instance.name);
        }
    }
}
