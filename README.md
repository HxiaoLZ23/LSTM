# 基于LSTM的文本情感分析系统

## 项目概述

这是一个基于C#和WPF的文本情感分析系统，使用ML.NET框架实现LSTM神经网络模型进行情感分析。系统采用现代化的架构设计，包含用户管理、文本分析、模型训练等功能。

## 技术栈

### 前端（UI层）
- **WPF**: 用户界面框架
- **Material Design**: 现代化UI设计
- **MVVM模式**: 数据绑定和命令模式
- **依赖注入**: Microsoft.Extensions.DependencyInjection

### 后端（业务逻辑层）
- **ML.NET**: 微软官方机器学习框架
- **Entity Framework Core**: 数据访问层
- **SQLite**: 轻量级数据库
- **AutoMapper**: 对象映射
- **BCrypt**: 密码加密

### 数据层
- **Repository模式**: 数据访问抽象
- **Unit of Work模式**: 事务管理
- **Entity Framework Core**: ORM框架

### 日志和配置
- **Serilog**: 结构化日志记录
- **Microsoft.Extensions.Configuration**: 配置管理
- **Microsoft.Extensions.Hosting**: 主机服务

## 项目结构

```
LSTM/
├── LSTM.sln                          # 解决方案文件
├── LSTM.Models/                      # 数据模型层
│   ├── User.cs                       # 用户实体
│   ├── SentimentAnalysis.cs          # 情感分析记录
│   ├── Dataset.cs                    # 数据集实体
│   ├── TrainingData.cs               # 训练数据
│   ├── MLModel.cs                    # 机器学习模型
│   └── DTOs/                         # 数据传输对象
├── LSTM.Data/                        # 数据访问层
│   ├── LSTMDbContext.cs              # 数据库上下文
│   ├── UnitOfWork.cs                 # 工作单元
│   ├── Interfaces/                   # 接口定义
│   └── Repositories/                 # 仓储实现
├── LSTM.ML/                          # 机器学习层
│   ├── Services/                     # ML服务实现
│   ├── Models/                       # ML数据模型
│   └── Interfaces/                   # ML接口
├── LSTM.Business/                    # 业务逻辑层
│   ├── Services/                     # 业务服务
│   └── Interfaces/                   # 业务接口
└── LSTM.UI/                          # 用户界面层
    ├── Views/                        # 视图
    ├── ViewModels/                   # 视图模型
    ├── Commands/                     # 命令
    └── App.xaml                      # 应用程序入口
```

## 核心功能

### 1. 用户管理
- **用户注册**: 创建新用户账户
- **用户登录**: 身份验证和授权
- **密码加密**: 使用BCrypt安全存储密码
- **会话管理**: 跟踪用户登录状态

### 2. 文本情感分析
- **实时分析**: 输入文本即时获得情感分析结果
- **多类别分类**: 支持积极、消极、中性三种情感分类
- **置信度评分**: 提供分析结果的可信度评分
- **历史记录**: 保存和查看分析历史

### 3. 模型训练
- **数据集管理**: 创建和管理训练数据集
- **数据导入**: 支持CSV、JSON、TXT格式的数据导入
- **模型训练**: 使用ML.NET训练LSTM模型
- **模型评估**: 评估模型准确率和性能
- **模型切换**: 加载不同的训练模型

### 4. 用户界面
- **现代化设计**: Material Design风格
- **响应式布局**: 适配不同屏幕尺寸
- **多标签页**: 分析、历史、训练功能分离
- **实时反馈**: 加载状态和错误提示

## 系统架构

### 分层架构
```
┌─────────────────────────────────────┐
│             UI Layer (WPF)          │
│         Views & ViewModels          │
├─────────────────────────────────────┤
│           Business Layer            │
│         Services & Logic            │
├─────────────────────────────────────┤
│         Machine Learning            │
│          ML.NET Services            │
├─────────────────────────────────────┤
│            Data Layer               │
│       Repository & UnitOfWork       │
├─────────────────────────────────────┤
│            Database                 │
│         SQLite with EF Core         │
└─────────────────────────────────────┘
```

### 依赖注入
系统使用Microsoft.Extensions.DependencyInjection进行依赖注入管理：
- 数据库上下文注册
- 服务层注册
- 视图模型注册
- 日志记录配置

## 数据库设计

### 主要表结构
- **Users**: 用户信息
- **Datasets**: 数据集
- **TrainingData**: 训练数据
- **MLModels**: 机器学习模型
- **SentimentAnalyses**: 情感分析记录

### 关系设计
- 一对多关系：User -> Datasets, User -> SentimentAnalyses
- 一对多关系：Dataset -> TrainingData, Dataset -> MLModels
- 多对一关系：SentimentAnalysis -> MLModel

## 机器学习实现

### ML.NET管道
1. **数据预处理**: 文本特征化
2. **模型训练**: 使用SdcaMaximumEntropy多分类器
3. **模型评估**: 计算准确率指标
4. **模型保存**: 序列化到文件
5. **模型加载**: 反序列化用于预测

### 备选方案
- **规则分析**: 基于关键词的简单情感分析
- **预训练模型**: 可扩展支持其他ML框架

## 运行要求

### 系统要求
- Windows 10 或更高版本
- .NET 6.0 Runtime
- 至少2GB内存
- 100MB可用磁盘空间

### 开发环境
- Visual Studio 2022
- .NET 6.0 SDK
- SQLite数据库

## 使用指南

### 首次运行
1. 编译并运行项目
2. 系统自动创建SQLite数据库
3. 使用默认账户登录（用户名：admin，密码：123456）

### 功能操作
1. **文本分析**: 在主界面输入文本，点击"分析"按钮
2. **查看历史**: 切换到"历史记录"标签页
3. **训练模型**: 
   - 创建数据集
   - 导入训练数据
   - 开始训练
   - 加载模型

### 数据格式
- **CSV**: text,sentiment
- **JSON**: [{"text": "...", "sentiment": "..."}]
- **TXT**: text\tsentiment（制表符分隔）

## 扩展功能

### 可扩展性
- 支持更多机器学习算法
- 集成外部API（如Azure认知服务）
- 支持批量文本处理
- 导出分析报告

### 优化建议
- 数据库索引优化
- 缓存机制
- 异步处理优化
- 内存管理优化

## 故障排除

### 常见问题
1. **数据库连接失败**: 检查SQLite文件权限
2. **模型训练失败**: 确认训练数据格式正确
3. **UI响应缓慢**: 检查是否有长时间运行的任务

### 日志查看
- 控制台输出：实时查看日志
- 文件日志：logs/app-{date}.log

## 更新日志

### V1.0.0
- 基础功能实现
- 用户管理系统
- 文本情感分析
- 模型训练接口
- WPF用户界面

## 开发团队

本项目基于用户需求设计和实现，采用现代化的C#技术栈，为文本情感分析提供完整的解决方案。

## 许可证

本项目仅供学习和研究使用。 
