# Screen Capture and Event Logger

## 项目简介

这个项目是一个基于 WinUI 3 的桌面应用程序，具有以下功能：
1. 每隔2秒自动截图一次，并将截图保存在 `D:\jianting` 目录下。
2. 监听并记录所有的键盘和鼠标事件（按键、鼠标点击），并将事件记录以 CSV 格式保存在 `D:\jianting` 目录下。

## 目录结构

├── App.xaml
├── App.xaml.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── ScreenCaptureHelper.cs
└── GlobalHookHelper.cs



### 文件说明

- `App.xaml`：定义应用程序的资源和启动配置。
- `App.xaml.cs`：处理应用程序的启动事件，创建并激活主窗口。
- `MainWindow.xaml`：定义主窗口的 UI。
- `MainWindow.xaml.cs`：实现主窗口的逻辑，包括启动截图功能和事件监听功能。
- `ScreenCaptureHelper.cs`：定义捕捉屏幕截图的功能。
- `GlobalHookHelper.cs`：定义全局钩子，用于监听键盘和鼠标事件。

## 功能说明

### 自动截图

每隔2秒钟自动截图一次，并将截图保存到 `D:\jianting` 目录下，文件名格式为 `Screenshot_yyyyMMdd_HHmmss.png`。

### 键盘和鼠标事件监听

监听所有的键盘和鼠标事件，包括：
- 键盘按键（按下和释放）
- 鼠标点击（左键和右键）

事件信息会被记录到 `D:\jianting\key_mouse_events.csv` 文件中，包含以下字段：
- `EventType`：事件类型（如 `KeyDown`、`KeyUp`、`LeftClick`、`RightClick`）
- `Timestamp`：事件发生的时间戳
- `KeyCode`：按键代码（仅键盘事件）
- `KeyChar`：按键字符（仅键盘事件）

## 安装与使用

### 前提条件

确保已经安装以下组件：
- .NET 6 SDK
- Visual Studio 2022 及以上版本
- Windows 10 SDK
- Windows 10 或更高版本

### NuGet 包

请确保安装以下 NuGet 包：
- `Microsoft.WindowsAppSDK`
- `System.Drawing.Common`

可以通过 NuGet 包管理器控制台或 Visual Studio 的 NuGet 包管理器界面安装这些包：

```bash
Install-Package Microsoft.WindowsAppSDK
Install-Package System.Drawing.Common


