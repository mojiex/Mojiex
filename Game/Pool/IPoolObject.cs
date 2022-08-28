using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex.Pool
{
    //CreateTime : 2022/8/26
    public interface IPoolObject
    {
        T Created<T>() where T : IPoolObject;
        void OnGet<T>(T value) where T : IPoolObject;
        void OnRelese<T>(T value)where T : IPoolObject;
        void OnDestory<T>(T value) where T : IPoolObject;
    }
}