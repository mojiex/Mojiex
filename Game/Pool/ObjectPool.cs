using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex.Pool
{
    //CreateTime : 2022/8/26
    public class ObjectPool<T> : IDisposable where T : IPoolObject
    {
        private T _poolObj;
        private Func<T> _createdFunc;
        private Action<T> _getAction;
        private Action<T> _releseAction;
        private Action<T> _destroyAction;

        private Stack<T> m_set;

        private int defaultSize;
        private int maxSize;
        private bool repeatCheck = true;
        public ObjectPool(Func<T> created, Action<T> get = null, Action<T> relese = null, Action<T> destroy = null, bool repeatCheck = true, int defaultSize = 10, int maxSize = 1000)
        {
            _createdFunc = created;
            _getAction = get;
            _releseAction = relese;
            _destroyAction = destroy;

            this.defaultSize = defaultSize;
            this.maxSize = maxSize;
            this.repeatCheck = repeatCheck;
            m_set = new Stack<T>(defaultSize);
        }

        public ObjectPool(T PoolObj, bool repeatCheck = true, int defaultSize = 10, int maxSize = 1000)
            : this(PoolObj.Created<T>, PoolObj.OnGet, PoolObj.OnRelese, PoolObj.OnDestory, repeatCheck, defaultSize, maxSize)
        {
            _poolObj = PoolObj;
        }
        public T Get()
        {
            T temp = (m_set.Count > 0) ? m_set.Pop() : _createdFunc();
            _getAction?.Invoke(temp);
            return temp;
        }

        public bool Relese(T value)
        {
            if(repeatCheck && m_set.Contains(value))
            {
                throw new AggregateException("value has been released and added to pool");
            }
            if(m_set.Count >= maxSize)
            {
                _destroyAction?.Invoke(value);
                return false;
            }

            m_set.Push(value);
            _releseAction?.Invoke(value);
            return true;
        }
        public void Dispose()
        {
            while(m_set.Count > 0)
            {
                _releseAction?.Invoke(m_set.Pop());
            }
            if (_poolObj != null)
            {
                _releseAction?.Invoke(_poolObj);
            }
        }
    }
}