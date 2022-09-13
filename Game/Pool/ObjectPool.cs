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
        private HashSet<T> m_usingSet;

        private int defaultSize;
        private int maxSize;
        private bool repeatCheck = true;

        public int CountAll => m_count;
        private int m_count = 0;
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
            m_usingSet = new HashSet<T>();
        }

        public ObjectPool(T PoolObj, bool repeatCheck = true, int defaultSize = 10, int maxSize = 1000)
            : this(PoolObj.Created<T>, PoolObj.OnGet, PoolObj.OnRelese, PoolObj.OnDestory, repeatCheck, defaultSize, maxSize)
        {
            _poolObj = PoolObj;
        }
        public T Get()
        {
            T temp;
            if ((m_set.Count > 0))
            {
                temp = m_set.Pop();
            }
            else
            {
                temp = _createdFunc();
                m_count++;
            }
            
            m_usingSet.Add(temp);
            _getAction?.Invoke(temp);
            return temp;
        }

        public bool Release(T value)
        {
            if(repeatCheck && m_set.Contains(value))
            {
                throw new System.ArgumentException("value has been released and added to pool");
            }
            if(m_set.Count >= maxSize)
            {
                _destroyAction?.Invoke(value);
                return false;
            }

            m_set.Push(value);
            m_usingSet.Remove(value);
            _releseAction?.Invoke(value);
            return true;
        }

        public void ReleaseAll()
        {
            foreach (var item in m_usingSet)
            {
                Release(item);
            }
        }

        public void DestoryAll()
        {
            if (_destroyAction == null)
            {
                MDebug.LogError("destory action not set");
                return;
            }
            ReleaseAll();
            while (m_set.Peek() != null)
            {
                _destroyAction(m_set.Pop());
            }
        }
        public void Dispose()
        {
            ReleaseAll();
            if(_destroyAction != null)
            {
                DestoryAll();
                _destroyAction(_poolObj);
            }
        }
    }
}