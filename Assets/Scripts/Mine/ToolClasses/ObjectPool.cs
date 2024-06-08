using System.Collections.Generic;
using UnityEngine;

namespace Mine.ToolClasses
{
    public class ObjectPool<T>where T : Component
    {
        private readonly T _mInitElement;
        private readonly Stack<T> _mElementStack;
        public ObjectPool(T initElement)
        {
            _mElementStack = new Stack<T>();
            _mInitElement = initElement;
        }

        public T Get()
        {
            var item = _mElementStack.Count == 0 ? Object.Instantiate(_mInitElement.gameObject).GetComponent<T>() : _mElementStack.Pop();

            item.gameObject.SetActive(true);
            return item;
        }

        public T Get(Transform transformParent)
        {
            var item = _mElementStack.Count == 0 ? Object.Instantiate(_mInitElement.gameObject).GetComponent<T>() : _mElementStack.Pop();
            
            Transform transform;
            (transform = item.transform).SetParent(transformParent);
            transform.localScale = Vector3.one;
            item.gameObject.SetActive(true);
            return item;
        }

        public void Recycle(T recycledElement)
        {
            recycledElement.gameObject.SetActive(false);
            _mElementStack.Push(recycledElement);
        }
        
    }
}
