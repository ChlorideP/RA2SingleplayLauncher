# 单关战役启动器

为了改良 ~~（抛弃）~~ YR的游戏界面，CNCNET社区开发出了spawner读图器，以及与之适配的XNA CnCNet客户端。

然鹅，一个单关任务捆绑上巨大多的遭遇战图，属实没有必要。而且，新版YR-CnCNet客户端为启动命令行耦合了-SPAWN参数，导致带Ares启动不得不另写批处理（但批处理与客户端的共存显然会引起混乱）

鉴于以上原因，咱依照HAres启动器的界面（取自[BV154411c7Ww](https://b23.tv/BV154411c7Ww)结尾测试部分）写了这么个单关战役的启动器。


# Braindead Launcher
**Braindead Launcher** 是由[**Caco**](https://github.com/CaconCaco)做**主程** [岛风](https://github.com/frg2089)协助制作的游戏启动器  
采用了最新版的 [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48) 框架
特点是
- 小, 编译出来的程序不到1MB
- 异步, 通过异步编程进一步提高用户体验
- 不支持XP, Windows XP操作系统最高仅支持 .NET Framework 4.0 版本
- 低耦合, <span style="color: #000; background-color: #000">我是有叫caco分层写的, 所以应该...大概...</span>可以只通过少量修改即可将启动器移植到您自己的项目中去

# 特别鸣谢
- **=Star=**
