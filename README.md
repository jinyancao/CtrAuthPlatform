# 简介

​	学习.NETCORE也有1年多时间了，发现.NETCORE项目实战系列教程很少，都是介绍开源项目或基础教程，对于那些观望的朋友不能形成很好的学习思路，遇到问题怕无法得到解决而不敢再实际项目中尝试，今天我想通过项目系列实战的方式，进一步推广应用.NETCORE，让大家感受它的魅力以及已经无所不能的神奇魔力，我会从实际项目开发的流程中带大家一起来学习和使用.NETCORE，对于项目实战系列写什么内容纠结很久，最后想想直接从基础设施开始着手，一步一步延伸到完整项目，第一篇就从统一身份认证模块开发详细介绍项目开发的过程。我也希望通过编写系列教程给自己巩固下学习成果，这是一个新东西，可参考的内容不多，我开发思路准备从原有项目中的一些实践使用.NETCORE来重构，在做的过程中也希望大家一起参与，集思广益、取长补短，共同完善好这个系列内容，做出一套精品教程为后来学习的人铺路。由于写这篇博文之前无任何项目代码作为参考，所以所有项目课程内容和源码都是在实际写作中编写，可能会遇到一些我无法解决的问题，也希望大家一起参与解决。

# 开发环境

​ 	**VS2017 .NETCORE2.1 WIN10 MSSQL2008R2**

# 使用的关键技术点

​    [.netcore 2.1](https://github.com/dotnet/core) 
​    [ocelot](https://ocelot.readthedocs.io/en/latest/) 
​    [identityserver4](https://identityserver4.readthedocs.io/en/release/)
​    [consul](https://www.consul.io/) 
​    [redis](https://redis.io/)
​    [dapper](https://github.com/StackExchange/Dapper)
​    [autofac](https://autofac.org/)
​    [automapper](https://github.com/AutoMapper/AutoMapper) 
​    [nginx](http://nginx.org/)
​    [docker](https://www.docker.com/)

# 目录（不定期更新）

## 后端篇

- [第一章 功能及架构分析](https://www.cnblogs.com/jackcao/p/9934970.html)
- [第二章 网关篇-定制Ocelot来满足需求](https://www.cnblogs.com/jackcao/p/9937213.html)
- [第三章 网关篇-数据库存储配置（1）](https://www.cnblogs.com/jackcao/p/9942561.html)
- [第四章 网关篇-数据库存储配置（2）](https://www.cnblogs.com/jackcao/p/9950305.html)
- [第五章 网关篇-自定义缓存REDIS](https://www.cnblogs.com/jackcao/p/9960788.html)
- [第六章 网关篇-自定义客户端授权](https://www.cnblogs.com/jackcao/p/9973765.html)
- [第七章 网关篇-自定义客户端限流](https://www.cnblogs.com/jackcao/p/9987424.html)
- [第八章 授权篇-IdentityServer4源码解析](https://www.cnblogs.com/jackcao/p/10031828.html)
- [第九章 授权篇-使用dapper重构IdenityServer4](https://www.cnblogs.com/jackcao/p/10058274.html)
- [第十章 授权篇-客户端授权模式](https://www.cnblogs.com/jackcao/p/10100621.html)
- [第十一章 授权篇-密码授权模式](https://www.cnblogs.com/jackcao/p/10140688.html)
- 第十二章 授权篇-自定义用户授权兼容老系统
- 第十三章 授权篇-验证码授权
- 第十四章 授权篇-QQ、微信等第三方授权
- 第十五章 授权篇-配合APP实现扫码登录（模拟）
- 第十六章 权限篇-用户权限设计
- 第十七章 权限篇-权限服务实现（内容等待定）
- 第十八章 整合篇-配合网关实现完整的后端服务（内容等待定）
