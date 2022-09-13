using System.Data;
using System;
using Mojiex;

[Serializable]
public class ArchipelagoData : IExcelData
{
	public int Id;
	public int[] islandBuilds;
	public string sceneName;
	public string name;
	public void FillData(DataRow data)
	{
		Id = int.Parse(data[0].ToString());
		islandBuilds = data[1].ToString().SplitByComma().ToInts();
		sceneName = data[2].ToString();
		name = data[3].ToString();
	}
}