using Mojiex.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mojiex
{
	public class UGUIListItem : MonoBehaviour
	{
		protected object _data;
		protected RectTransform rectTransform;
		public virtual void Awake()
        {
			rectTransform = transform as RectTransform;

		}

        public virtual void UpdateUI(object obj)
        {
			_data = obj;
        }
	}
	
}