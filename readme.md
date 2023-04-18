---
有需要请联系我  monsterofmj@163.com
---
---
**需要unity的addressable包（com.unity.addressables）**
---
# UI ----
### Ⅰ、创建UI脚本
+ gameObject->MUITool->CreatePanelScript可以新建选中物体的UI脚本
+ 新建的panel脚本保存在Assets/_Script/UI
+ gameObject->MUITool->CreateItemScript可以新建选中物体的UI脚本
+ 新建的Item脚本保存在Assets/_Script/UI/Item
+ 该操作会新建两个脚本，一个命名和选中的物体一样，做逻辑，另一个为选中的物体名+com，用于获取物体上的组件
+ 重复该操作会更新com文件，但不会更新同名脚本
+ 在物体的同名脚本中，重写无参数构造函数并更改currentSortLayer和LayerPriority更改层级
+ 为onDestroy添加的监听可以在该ui被关闭时响应
### Ⅱ、使用创建的UI
+ 调用Mgr.uiMgr.Add\<T\>();其中T是您需要添加的UI的类型（在调用之前请确保您已经初始化过Mgr）
+ 关闭UI请使用Mgr.uiMgr.Close\<T\>();其中T是您需要关闭的UI的类型
+ 添加遮罩请使用Mgr.uiMgr.AddMask(T uiObj);其中T您的目标物体，该操作会在物体层级之下生成半透明黑色图片
+ 遮罩可以在物体销毁时自动关闭，不必手动调用Mgr.uiMgr.CloseMask(T uiObj);
+ Esc关闭最上层ui(暂未完成 2022/08/28)，可以调整EscClose指定是否响应Esc键
---
# Data ----
***重要:在只修改数据类型而不修改数据名时，如果已经有保存的数据，读取时会导致类型转换错误***
***清除数据重新运行会解决问题***
***但是非常不建议更改数据类型而不更改数据名，正确做法是声明新的类型新的变量代替，而不是直接更改数据类型***
### Ⅰ、数据保存形式
+ 该项目使用了LitJson将类中的公共类型变量转换为Json数据类型
+ 使用的C#自带的Ase加密，key在Const类中定义，IV在PlayerPrefabHelper中定义
+ 需要保存的类需要继承BaseSaveInfo，每个类只能有一个实例可以被保存
### Ⅱ、读取、保存数据
+ 调用Mgr.saveMgr.Load\<T\>();读取数据
+ 调用Mgr.saveMgr.Save\<T\>(T data);保存数据
+ 读取数据时如果没有数据会返回T类型调用InitDefault();的结果
---
# EventSystem ----
***方法都是静态方法，直接使用类名调用即可***
+ 使用时需要声明类型T，T是enum
+ 注册事件调用EventSystem\<T\>.AddListener(T key, Action\<object[]\> CallBack)
+ 注销事件的单个响应调用EventSystem\<T\>.RemoveListener(T key, Action\<object[]\> CallBack)
+ 注销整个事件的响应调用EventSystem\<T\>.RemoveListener(T key)
+ 注销该System的事件调用EventSystem\<T\>.RemoveListener()
---
# Excel ----
***不支持Excel文件的修改***

***Excel第1行是备注，第二行标注数据名和数据类型，程序会从第三行开始读取数据***

***目前支持的数据类型有int,int[],float,float[],string,string[]***
+ 通过添加 [] 来表示数组，例如int[]表示int数组
+ 第二行示例 ***id:int*** 读取后生成字段{public int id;}
***ArchipelagoData.xlsx及其同名文件用作测试，可以删除***
### Excel文件存放
+ Excel文件存放于Assets/Mojiex/Excel/File目录下，仅支持.xlsx后缀文件
### 读取Excel
+ 在文件放入上述目录下之后，点击工具栏Excel/ReadExcel后会生成excel同名的.cs文件，并存放于Assets//Mojiex/Excel/Script目录下
+ Debug栏显示"Excel Read Complete!"表示读取完成
+ 读取后会在Assets//Mojiex/Excel/Asset/下生成ExcelDataHandler二进制加密后的数据文件
+ 重复读取会获取覆盖之前的数据
### 使用已读取的数据
+ 使用DictionaryStatic单例类来统一使用已经保存的数据
+ 如果有新的数据加入请在DictionaryStatic中加入以下格式代码
```c#
{
    //请将代码中的Data字符串替换为您新加的excel表名
    public List<Data> GetAllDatas() => excelData.m_Data;
}
```
```C#
{ 
    //使用时可以参考以下代码
    //请将代码中的Data字符串替换为您新加的excel表名
    List<Data> Datas =DictionaryStatic.Inst.GetAllDatas();
}
```

