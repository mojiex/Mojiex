using Mojiex.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
	public class UGUIListItem : MonoBehaviour,IDisposable,IPoolObject
	{
		protected object _data;
		protected RectTransform rectTransform;
        public int Index { get; protected set; } = 0;
        public new GameObject gameObject;
		public virtual void Awake()
        {
			rectTransform = transform as RectTransform;
            gameObject = base.gameObject;
		}

        public virtual T Created<T>() where T : IPoolObject
        {
            return GameObject.Instantiate(gameObject).GetComponent<T>();
        }

        public virtual void OnDestory<T>(T value) where T : IPoolObject
        {
            (value as UGUIListItem).Dispose();
        }

        public virtual void OnGet<T>(T value) where T : IPoolObject { }

        public virtual void OnRelese<T>(T value) where T : IPoolObject
        {
            (value as UGUIListItem).gameObject.SetActive(false);
        }

        public virtual void UpdateUI(int index, object obj)
        {
            Index = index;
            _data = obj;
        }

        public virtual void Select(int index) { }

        public virtual void Dispose()
        {
            Destroy(gameObject);
        }
    }
	
}