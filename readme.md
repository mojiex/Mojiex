# UI----
## Ⅰ、创建UI脚本
### 1、gameObject->MUITool->CreatUIScript可以新建选中物体的UI脚本
### 2、新建的脚本保存在Assets/_Script/UI
### 3、该操作会新建两个脚本，一个命名和选中的物体一样，做逻辑，另一个为选中的物体名+com，用于获取物体上的组件
### 4、重复该操作会更新com文件，但不会更新同名脚本
### 5、在物体的同名脚本中，重写无参数构造函数并更改currentSortLayer和LayerPriority更改层级
### 6、为onDestroy添加的监听可以在该ui被关闭时响应
## Ⅱ、使用创建的UI
### 1、调用Mgr.uiMgr.Add<T>();其中T是您需要添加的UI的类型（在调用之前请确保您已经初始化过Mgr）
### 2、关闭UI请使用Mgr.uiMgr.Close<T>();其中T是您需要关闭的UI的类型
### 3、添加遮罩请使用Mgr.uiMgr.AddMask(T uiObj);其中T您的目标物体，该操作会在物体层级之下生成半透明黑色图片
### 4、遮罩可以在物体销毁时自动关闭，不必手动调用Mgr.uiMgr.CloseMask(T uiObj);
### 5、Esc关闭最上层ui(暂未完成 2022/08/28)，可以调整EscClose指定是否响应Esc键
# Data----
