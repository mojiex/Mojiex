using System.Data;
using System;
using Mojiex;

[Serializable]
public class LocalizationData : IExcelData
{
	public string Key;
	public string English;
	public string Chinese;
	public string ChineseSimplified;
	public string Korean;
	public string Japanese;
	public string French;
	public string German;
	public string Russian;
	public void FillData(DataRow data)
	{
		Key = data[0].ToString();
		English = data[1].ToString();
		Chinese = data[2].ToString();
		ChineseSimplified = data[3].ToString();
		Korean = data[4].ToString();
		Japanese = data[5].ToString();
		French = data[6].ToString();
		German = data[7].ToString();
		Russian = data[8].ToString();
	}
}