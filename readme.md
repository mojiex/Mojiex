---
有需要请联系我  monsterofmj@163.com
---
---
# UI ----
### Ⅰ、创建UI脚本
+ gameObject->MUITool->CreatUIScript可以新建选中物体的UI脚本
+ 新建的脚本保存在Assets/_Script/UI
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
