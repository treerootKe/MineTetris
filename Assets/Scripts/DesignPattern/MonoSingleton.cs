
using System;
using UnityEngine;

namespace DesignPattern
{
    public class MonoSingleton<T>: MonoBehaviour where T:MonoBehaviour
    {
        private static T _instance;

        protected static T Instance => _instance;
        protected virtual void Awake()
        {
            _instance = transform.GetComponent<T>();
        }
    }
}
