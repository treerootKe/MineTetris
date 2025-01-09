
using System;
using UnityEngine;

namespace DesignPattern
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            Instance = transform.GetComponent<T>();
        }
    }
}
