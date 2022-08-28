using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    //CreateTime : 2022/8/27
    public class BaseSaveInfo
    {
        public BaseSaveInfo()
        {
            InitDefault();
        }
        //only save public variable
        //只保存public类型变量
        public virtual void InitDefault()
        {
            //set default data,the initialized value in here should be set to the same value in any case
            //设置默认值，在这里初始化的值在任何情况下都要设置为相同的值
        }

        public void Fill(object data)
        {
            var savedProps = data.GetType().GetFields();
            var selfProps = this.GetType().GetFields();
            int[] abc = new int[] { };
            foreach (var item in savedProps)
            {
                foreach (var selfItem in selfProps)
                {
                    if (selfItem.Name.Equals(item.Name) && item.GetValue(data) != null)
                    {
                        if (selfItem.GetValue(this) is IList)
                        {
                            var selfList = (IList)selfItem.GetValue(this);
                            var list = (IList)item.GetValue(data);
                            if ((list).Count > 0)
                            {
                                if (list[0] is BaseSaveInfo)
                                {
                                    for (int i = 0; i < list.Count; i++)
                                    {
                                        if (i < selfList.Count)
                                        {
                                            ((BaseSaveInfo)selfList[i]).Fill(list[i]);
                                        }
                                        else
                                        {
                                            Type type = list[i].GetType();
                                            object obj = type.Assembly.CreateInstance(type.ToString());
                                            ((BaseSaveInfo)obj).InitDefault();
                                            ((BaseSaveInfo)obj).Fill(list[i]);
                                            selfList.Add(obj);
                                        }
                                    }
                                }
                                else
                                {
                                    selfItem.SetValue(this, list);
                                }
                            }
                        }
                        else if (selfItem.GetValue(this) is Array)
                        {
                            var selfArray = (Array)selfItem.GetValue(this);
                            var array = (Array)item.GetValue(data);
                            if (array.Length > 0)
                            {
                                if (array.GetValue(0) is BaseSaveInfo)
                                {
                                    for (int i = 0; i < array.Length; i++)
                                    {
                                        ((BaseSaveInfo)(selfArray.GetValue(i))).Fill(array.GetValue(i));
                                    }
                                }
                                else
                                {
                                    selfItem.SetValue(this, array);
                                }
                            }
                        }
                        else
                        {
                            if (selfItem.GetValue(this) is BaseSaveInfo)
                            {
                                ((BaseSaveInfo)selfItem.GetValue(this)).Fill(item.GetValue(data));
                            }
                            else
                            {
                                selfItem.SetValue(this, item.GetValue(data));
                            }
                        }
                    }
                }
            }
        }
    }
}