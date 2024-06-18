
using System;
using UnityEngine;

namespace Tetris.DesignPattern
{
    public class MonoSingleton<T>: MonoBehaviour where T:MonoBehaviour
    {
        private static T instance;

        protected static T Instance => instance;
        protected virtual void Awake()
        {
            instance = transform.GetComponent<T>();
            Debug.Log(instance.name);
        }
    }
}
