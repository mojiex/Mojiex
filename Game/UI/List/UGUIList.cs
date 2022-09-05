using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mojiex
{
	public class UGUIList : MonoBehaviour
	{
		public GameObject Prefab;
        public System.Type _dataType;

		public object[] Data
        {
            get => _data;
            set
            {
                if (value == null)
                    return;
                _data = value;
                UpdateView(_data);
            }
        }
        private object[] _data = null;

        public int DataCount => _data.Length;
        private List<UGUIListItem> itemPool = new List<UGUIListItem>();

        public virtual void UpdateView(object[] data) => UpdateView(Prefab, data);
        public virtual void UpdateView(GameObject prefab, object[] data)
        {
            if(prefab == null)
            {
                throw new System.NullReferenceException($"prefab is null");
            }
            while(DataCount > itemPool.Count)
            {
                if (GameObject.Instantiate(prefab,transform).TryGetComponent(out UGUIListItem item))
                {
                    itemPool.Add(item);
                }
                else
                {
                    throw new System.NullReferenceException("UGUIListItem not be found in prefab");
                }
            }
            while(DataCount < itemPool.Count)
            {
                itemPool[itemPool.Count - 1].gameObject.SetActive(false);
            }

            for (int i = 0; i < data.Length; i++)
            {
                itemPool[i].UpdateUI(data[i]);
                itemPool[i].gameObject.SetActive(true);
            }

            if (TryGetComponent(out UnityEngine.UI.LayoutGroup _))
            {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
            }
        }
	}
	
}