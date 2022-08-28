using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex.Pool
{
    //CreateTime : 2022/8/26
    public class PoolManager : IMgr
    {
        private Dictionary<string, ObjectPool<IPoolObject>> poolDic;
        public void Dispose()
        {
            foreach (var item in poolDic)
            {
                item.Value.Dispose();
            }
            poolDic.Clear();
        }

        public void Init()
        {
            poolDic = new Dictionary<string, ObjectPool<IPoolObject>>();
        }

        public bool AddPool(string key,ObjectPool<IPoolObject> pool)
        {
            if (poolDic.ContainsKey(key))
            {
                return false;
            }
            poolDic.Add(key, pool);
            return true;
        }

        public ObjectPool<IPoolObject> GetPool(string key)
        {
            if (!poolDic.ContainsKey(key))
            {
                throw new NullReferenceException($"poolDic do not contain key : {key}");
            }
            return poolDic[key];
        }

        public bool HasPool(string key)
        {
            return poolDic.ContainsKey(key);
        }

        public bool IsInited()
        {
            return poolDic != null;
        }
    }
}