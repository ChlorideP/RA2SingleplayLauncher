# 单关战役启动器

为了改良 ~~（抛弃）~~ YR的游戏界面，CNCNET社区开发出了spawner读图器，以及与之适配的XNA CnCNet客户端。

然鹅，一个单关任务为了适配这个客户端捆绑上巨大多的遭遇战图，属实没有必要。
另一方面，没有启动器，只靠一个批处理也难以调控难度和纷杂的渲染补丁。

鉴于以上原因，咱依照HAres启动器的界面（取自[BV154411c7Ww](https://b23.tv/BV154411c7Ww)结尾测试部分）写了这么个单关战役的启动器。


# Battle Launcher
**Battle Launcher** 是由 [**Caco**](https://github.com/CaconCaco) 做**整体构架**，[岛风](https://github.com/frg2089)协助的红色警戒2单关战役启动器

目前已适配咱的「脑死」<del>任务</del>mod。

采用了最新版的 [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48) 框架，
特点是
- 小, 编译出来的程序不到1MB
- 异步, 通过异步编程进一步提高用户体验
- 不支持XP, Windows XP操作系统最高仅支持 .NET Framework 4.0 版本
- 低耦合, ~~我是有叫caco分层写的, 所以应该...大概...~~ 可以只通过少量修改即可将启动器移植到您自己的项目中去

# 特别鸣谢
- [**=Star=**](https://space.bilibili.com/4920382)
