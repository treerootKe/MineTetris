using System.Collections.Generic;
using UnityEngine;

namespace Tetris.ToolClasses
{
    public class ObjectPool<T>where T : Component
    {
        private readonly T _mInitObject;
        private readonly Stack<T> _mObjectStack;
        public ObjectPool(T initElement)
        {
            _mObjectStack = new Stack<T>();
            _mInitObject = initElement;
        }

        public T Get()
        {
            var item = _mObjectStack.Count == 0 ? Object.Instantiate(_mInitObject.gameObject).GetComponent<T>() : _mObjectStack.Pop();

            item.gameObject.SetActive(true);
            return item;
        }

        public T Get(Transform transformParent)
        {
            var item = _mObjectStack.Count == 0 ? Object.Instantiate(_mInitObject.gameObject).GetComponent<T>() : _mObjectStack.Pop();

            Transform transform;
            (transform = item.transform).SetParent(transformParent);
            transform.localScale = Vector3.one;
            item.gameObject.SetActive(true);
            return item;
        }

        public void Recycle(T recycledElement)
        {
            recycledElement.gameObject.SetActive(false);
            _mObjectStack.Push(recycledElement);
        }
        
    }
}
