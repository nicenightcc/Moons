# Moons
dotnet core 快速开发微服务框架
key words: 微服务 依赖注入 适配器模式 代理模式 

# 优点：
将mvvm中数据层和业务层简化，使用扩展性良好的微服务架构，框架提供可替换基础模块，让开发者专注于业务逻辑编写，大多只需编写数据库模型工程和webapi接口工程。

# 缺点：
需将项目按领域划分为多个服务，服务间无法直接互相读取数据库，需要提供api接口。启动时反射需要一些时间

# 结构目录
Microservices.Base 基础结构工程
包括依赖注入模块，反射用接口（IDefinition、IAdapter、IHandler、IRequest、IResponse），继承这些接口即可调用反射。

Microservices.Adapters 适配器工程
数据库模型工程需引用Microservices.Adapters.IDatabase，webapi接口工程需引用Microservices.Adapters.IWebApi

Microservices.WebApi webapi服务器工程
提供webapi服务器反射加载支持，使用Kestrel

# 已完成：
1.基础接口 IDefinition、IAdapter、IHandler、IRequest、IResponse、ICommand
2.依赖注入模块 IOCAdapter
3.适配器工厂 AdapterFac
4.适配器 MySQL、Redis
5.webapi服务器工程 Microservices.WebApi

# 施工中：
1.适配器 RabbitMQAdapter、SignalRAdapter
2.消息队列 消费者工程 Microservices.MessageQueue
3.SignlR Hub服务器工程 Microservices.SignlR
3.服务器控制台工程 Microservices.Command

# 已知bug：
1.高并发情况下MySqlAdapter可能出现链接占用未释放情况
2.因使用dotnetcore EF版本，在数据库模型工程中标识外键无效
