using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Mojiex
{
    public class ScrollLimiterList : ScrollLimiterRect
    {
        public GameObject Item;
        public int ItemCount
        {
            get => itemCount;
            set
            {
                itemCount = value;
                if (!Initial)
                {
                    Init();
                }
                UpdateAnchoredPositionList();
            }
        }

        private  int itemCount;

        private bool Initial = false;

        private int SelectedId = -1;

        private List<Vector2> anchoredPositionList;
        public override void Awake()
        {

        }

        protected override void Init()
        {
            base.Init();
            anchoredPositionList = new List<Vector2>();
        }

        protected void UpdateAnchoredPositionList()
        {
            Vector2 OrignSize;
            if (Item != null && Item.TryGetComponent(out RectTransform rect))
            {
                Vector2 size = OrignSize = rect.rect.size;
                if (TryGetComponent(out HorizontalOrVerticalLayoutGroup layout))
                {
                    size += new Vector2(layout.spacing, layout.spacing);
                }
                if (TryGetComponent(out GridLayoutGroup grid))
                {
                    size += grid.spacing;
                }
                HorizonGap = (int)size.x;
                VerticalGap = (int)size.y;
            }
            else
            {
                MDebug.LogError("Item is null or is not RectTransform");
                return;
            }

            anchoredPositionList.Clear();
            if (ItemCount <= 0)
            {
                return;
            }
            Vector2 basePos = new Vector2(m_init.x,m_init.y);
            basePos.x = Horizon ? maxX : 0;
            basePos.y = Vertical ? maxY : 0;
            for (int i = 0; i < ItemCount; i++)
            {
                anchoredPositionList.Add(basePos);
                //float t = basePos.x - HorizonGap;
                //float y = Mathf.Abs(basePos.x - HorizonGap - minX);
                //float u = HorizonGap - OrignSize.x;

                if (basePos.x - HorizonGap >= minX)
                {
                    basePos.x -= HorizonGap;
                }
                else if (Mathf.Abs(basePos.x - HorizonGap - minX) == (HorizonGap - OrignSize.x))
                {
                    basePos.x = minX;
                }
                else if (basePos.y - VerticalGap >= minY)
                {
                    basePos.y -= VerticalGap;
                    if (Horizon && Vertical)
                        basePos.x = maxX;
                }
                else if (Mathf.Abs(basePos.y - VerticalGap - minY) == (VerticalGap - OrignSize.y))
                {
                    basePos.y = minY;
                }
                else
                {
                    if (anchoredPositionList.Count != ItemCount)
                        MDebug.LogError("item out of move range");
                }
            }
        }

        public void Select(int index)
        {
            if (index >= ItemCount || index < 0)
            {
                Debug.LogError($"index out of bound 0 ~ {ItemCount}");
                return;
            }
            SelectedId = index;
            if (Horizon)
                m_targetPos.x = anchoredPositionList[index].x;
            if (Vertical)
                m_targetPos.y = anchoredPositionList[index].y;
            updatePos = true;
        }
        public int GetSelected()
        {
            return SelectedId;
        }
    }
}
