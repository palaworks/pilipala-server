# 第一章 - HelloWorld in pilipala

> 本章将概述如何快速构建你的第一个pilipala示例应用。

首先下载pilipala-example例程，并确保您的设备拥有简介中的环境条件。

## 1.1 基本部署

### pilipala配置文件的设定

pilipala的配置信息存储在appsettings.json中，以下代码演示了一个基本的配置信息。

<pre><code>{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "AppSettings": {
    "Database": {
      "Connection": {
        "User": "root",
        "PWD": "65a1561425f744e2b541303f628963f8",
        "DataSource": "localhost",
        "Port": "3306"
      },

      "Meta": {
        "Name": "pilipala",
        "Tables": {
          "User": "pl_user",
          "Index": "pl_index",
          "Backup": "pl_backup",
          "Archive": "pl_archive",
          "Comment": "comment_lake"
        },
        "ViewsSet": {
          "CleanViews": {
            "PosUnion": "pos>union",
            "NegUnion": "neg>union"
          },
          "DirtyViews": {
            "PosUnion": "pos>dirty>union",
            "NegUnion": "neg>dirty>union"
          }
        }
      }
    },
    "User": {
      "Account": "1951327599",
      "PWD": "thaumy12384"
    },

    "Theme": {
      "Path": "wwwroot/field_config.json"
    }
  }
}</code></pre>

`Database`节点包含了噼里啪啦的数据库信息，涵盖连接配置和库表元信息两个节点。  
`User`节点包含了噼里啪啦的管理员用户信息，此信息规定了哪些用户可以充当管理员。  
`Theme`节点包含了主题配置，`Path`子节点规定了主题配置文件的路径位置。

### 示例数据库的导入

请导入例程中的`exampala.sql`。

### 尝试运行pilipala

Linux/macOS环境：`dotnet PILIPALA.dll --urls http://0.0.0.0:5000`
Windows环境：运行`PILIPALA.exe`

## 1.2 扩展部署

pilipala为操作文章提供了丰富的WebAPI交换接口，允许以安全可靠的方式架设后台。  
其中，jiligulu是pilipala的样本后台，它通过调用pilipala的一系列交互接口与主机进行通信。

### 叽里咕噜配置文件的设置

叽里咕噜的配置文件为`?.json`，它涵盖了目标pilipala站点的一些公开信息，并以此为基础展开和主机的通信。  
叽里咕噜是基于Electron的跨平台应用，无需任何环境配置即可运行。
