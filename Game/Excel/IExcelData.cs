#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/9/12
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Data;

namespace Mojiex
{
    public interface IExcelData
    {
        void FillData(DataRow data);
    }
}