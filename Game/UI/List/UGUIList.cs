using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mojiex
{
    public class UGUIList : MonoBehaviour
    {
        public UGUIListItem Prefab
        {
            get => m_prefab;
            set
            {
                if (value == null)
                {
                    MDebug.LogError("NullReferenceException value is null");
                }
                m_prefab = value;
                if (_objPool != null)
                {
                    _objPool.Dispose();
                }
                _objPool = new Pool.ObjectPool<UGUIListItem>(Creat, value.OnGet, value.OnRelese, value.OnDestory);
                itemList.Clear();
                if (Data != null && Data.Length > 0)
                {
                    itemList.Add(_objPool.Get());
                    UpdateView(_data);
                }
            }
        }
        [SerializeField]
        private UGUIListItem m_prefab;

        public int Select
        {
            get => m_select;
            set
            {
                if (value < 0)
                {
                    MDebug.LogError("selected index can not be less than zero.");
                }
                m_select = value;
                for (int i = 0; i < itemList.Count; i++)
                {
                    itemList[i].Select(m_select);
                }
            }
        }
        private int m_select = -1;

        public virtual object[] Data
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

        public UGUIListItem this[int index]
        {
            get
            {
                return itemList[index];
            }
        }

        public int DataCount => _data.Length;
        protected List<UGUIListItem> itemList = new List<UGUIListItem>();
        private Pool.ObjectPool<UGUIListItem> _objPool;

        public virtual void UpdateView(object[] data) => UpdateView(Prefab, data);
        public virtual void UpdateView(UGUIListItem prefab, object[] data)
        {
            if (prefab == null)
            {
                MDebug.LogError($"prefab is null");
                return;
            }
            try
            {
                prefab.gameObject.SetState(false);
            }
            catch (System.Exception) { }
            // if (prefab.gameObject.activeSelf)
            // {
            //     prefab.gameObject.SetActive(false);
            // }
            if (_objPool == null)
            {
                _objPool = new Pool.ObjectPool<UGUIListItem>(Creat, prefab.OnGet, prefab.OnRelese, prefab.OnDestory);
            }
            while (DataCount > itemList.Count)
            {
                itemList.Add(_objPool.Get());
            }
            while (DataCount < itemList.Count)
            {
                _objPool.Release(itemList[itemList.Count - 1]);
            }

            for (int i = 0; i < data.Length; i++)
            {
                itemList[i].UpdateUI(i, data[i]);
                itemList[i].gameObject.SetActive(true);
            }

            if (TryGetComponent(out UnityEngine.UI.LayoutGroup _))
            {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
            }
        }

        public UGUIListItem GetSelect()
        {
            if (m_select < 0 && m_select >= itemList.Count)
            {
                MDebug.LogError("selected index out of bound 0~Data.Length");
                return null;
            }
            return itemList[m_select];
        }

        private UGUIListItem Creat()
        {
            UGUIListItem Item = GameObject.Instantiate(Prefab, transform).GetComponent<UGUIListItem>();
            Item.Awake();
            return Item;
        }

    }

}