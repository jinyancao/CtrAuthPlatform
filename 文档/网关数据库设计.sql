/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2018/11/10 17:14:21                          */
/*==============================================================*/


if exists (select 1
            from  sysindexes
           where  id    = object_id('AhphConfigReRoutes')
            and   name  = 'Relationship_5_FK'
            and   indid > 0
            and   indid < 255)
   drop index AhphConfigReRoutes.Relationship_5_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('AhphConfigReRoutes')
            and   name  = 'Relationship_4_FK'
            and   indid > 0
            and   indid < 255)
   drop index AhphConfigReRoutes.Relationship_4_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('AhphConfigReRoutes')
            and   type = 'U')
   drop table AhphConfigReRoutes
go

if exists (select 1
            from  sysobjects
           where  id = object_id('AhphGlobalConfiguration')
            and   type = 'U')
   drop table AhphGlobalConfiguration
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('AhphReRoute')
            and   name  = '分类路由信息_FK'
            and   indid > 0
            and   indid < 255)
   drop index AhphReRoute.分类路由信息_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('AhphReRoute')
            and   type = 'U')
   drop table AhphReRoute
go

if exists (select 1
            from  sysobjects
           where  id = object_id('AhphReRoutesItem')
            and   type = 'U')
   drop table AhphReRoutesItem
go

/*==============================================================*/
/* Table: AhphConfigReRoutes                                    */
/*==============================================================*/
create table AhphConfigReRoutes (
   CtgRouteId           int                  identity,
   AhphId               int                  null,
   ReRouteId            int                  null,
   constraint PK_AHPHCONFIGREROUTES primary key nonclustered (CtgRouteId)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '网关-路由,可以配置多个网关和多个路由',
   'user', @CurrentUser, 'table', 'AhphConfigReRoutes'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '配置路由主键',
   'user', @CurrentUser, 'table', 'AhphConfigReRoutes', 'column', 'CtgRouteId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '网关主键',
   'user', @CurrentUser, 'table', 'AhphConfigReRoutes', 'column', 'AhphId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '路由主键',
   'user', @CurrentUser, 'table', 'AhphConfigReRoutes', 'column', 'ReRouteId'
go

/*==============================================================*/
/* Index: Relationship_4_FK                                     */
/*==============================================================*/
create index Relationship_4_FK on AhphConfigReRoutes (
AhphId ASC
)
go

/*==============================================================*/
/* Index: Relationship_5_FK                                     */
/*==============================================================*/
create index Relationship_5_FK on AhphConfigReRoutes (
ReRouteId ASC
)
go

/*==============================================================*/
/* Table: AhphGlobalConfiguration                               */
/*==============================================================*/
create table AhphGlobalConfiguration (
   AhphId               int                  identity,
   GatewayName          varchar(100)         not null,
   RequestIdKey         varchar(100)         null,
   BaseUrl              varchar(100)         null,
   DownstreamScheme     varchar(50)          null,
   ServiceDiscoveryProvider varchar(500)         null,
   LoadBalancerOptions  varchar(500)         null,
   HttpHandlerOptions   varchar(500)         null,
   QoSOptions           varchar(200)         null,
   IsDefault            int                  not null default 0,
   InfoStatus           int                  not null default 1,
   constraint PK_AHPHGLOBALCONFIGURATION primary key nonclustered (AhphId)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '网关全局配置表',
   'user', @CurrentUser, 'table', 'AhphGlobalConfiguration'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '网关主键',
   'user', @CurrentUser, 'table', 'AhphGlobalConfiguration', 'column', 'AhphId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '网关名称',
   'user', @CurrentUser, 'table', 'AhphGlobalConfiguration', 'column', 'GatewayName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '全局请求默认key',
   'user', @CurrentUser, 'table', 'AhphGlobalConfiguration', 'column', 'RequestIdKey'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '请求路由根地址',
   'user', @CurrentUser, 'table', 'AhphGlobalConfiguration', 'column', 'BaseUrl'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '下游使用架构',
   'user', @CurrentUser, 'table', 'AhphGlobalConfiguration', 'column', 'DownstreamScheme'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '服务发现全局配置,值为配置json',
   'user', @CurrentUser, 'table', 'AhphGlobalConfiguration', 'column', 'ServiceDiscoveryProvider'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '全局负载均衡配置',
   'user', @CurrentUser, 'table', 'AhphGlobalConfiguration', 'column', 'LoadBalancerOptions'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Http请求配置',
   'user', @CurrentUser, 'table', 'AhphGlobalConfiguration', 'column', 'HttpHandlerOptions'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '请求安全配置,超时、重试、熔断',
   'user', @CurrentUser, 'table', 'AhphGlobalConfiguration', 'column', 'QoSOptions'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否默认配置, 1 默认 0 默认',
   'user', @CurrentUser, 'table', 'AhphGlobalConfiguration', 'column', 'IsDefault'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '当前状态, 1 有效 0 无效',
   'user', @CurrentUser, 'table', 'AhphGlobalConfiguration', 'column', 'InfoStatus'
go

/*==============================================================*/
/* Table: AhphReRoute                                           */
/*==============================================================*/
create table AhphReRoute (
   ReRouteId            int                  identity,
   ItemId               int                  null,
   UpstreamPathTemplate varchar(150)         not null,
   UpstreamHttpMethod   varchar(50)          not null,
   UpstreamHost         varchar(100)         null,
   DownstreamScheme     varchar(50)          not null,
   DownstreamPathTemplate varchar(200)         not null,
   DownstreamHostAndPorts varchar(500)         null,
   AuthenticationOptions varchar(300)         null,
   RequestIdKey         varchar(100)         null,
   CacheOptions         varchar(200)         null,
   ServiceName          varchar(100)         null,
   LoadBalancerOptions  varchar(500)         null,
   QoSOptions           varchar(200)         null,
   DelegatingHandlers   varchar(200)         null,
   Priority             int                  null,
   InfoStatus           int                  not null default 1,
   constraint PK_AHPHREROUTE primary key nonclustered (ReRouteId)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '路由配置表',
   'user', @CurrentUser, 'table', 'AhphReRoute'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '路由主键',
   'user', @CurrentUser, 'table', 'AhphReRoute', 'column', 'ReRouteId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '分类主键',
   'user', @CurrentUser, 'table', 'AhphReRoute', 'column', 'ItemId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '上游路径模板，支持正则',
   'user', @CurrentUser, 'table', 'AhphReRoute', 'column', 'UpstreamPathTemplate'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '上游请求方法数组格式',
   'user', @CurrentUser, 'table', 'AhphReRoute', 'column', 'UpstreamHttpMethod'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '上游域名地址',
   'user', @CurrentUser, 'table', 'AhphReRoute', 'column', 'UpstreamHost'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '下游使用架构',
   'user', @CurrentUser, 'table', 'AhphReRoute', 'column', 'DownstreamScheme'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '下游路径模板,与上游正则对应',
   'user', @CurrentUser, 'table', 'AhphReRoute', 'column', 'DownstreamPathTemplate'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '下游请求地址和端口,静态负载配置',
   'user', @CurrentUser, 'table', 'AhphReRoute', 'column', 'DownstreamHostAndPorts'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '授权配置,是否需要认证访问',
   'user', @CurrentUser, 'table', 'AhphReRoute', 'column', 'AuthenticationOptions'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '全局请求默认key',
   'user', @CurrentUser, 'table', 'AhphReRoute', 'column', 'RequestIdKey'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '缓存配置,常用查询和再次配置缓存',
   'user', @CurrentUser, 'table', 'AhphReRoute', 'column', 'CacheOptions'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '服务发现名称,启用服务发现时生效',
   'user', @CurrentUser, 'table', 'AhphReRoute', 'column', 'ServiceName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '全局负载均衡配置',
   'user', @CurrentUser, 'table', 'AhphReRoute', 'column', 'LoadBalancerOptions'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '请求安全配置,超时、重试、熔断',
   'user', @CurrentUser, 'table', 'AhphReRoute', 'column', 'QoSOptions'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '委托处理方法,特定路由定义委托单独处理',
   'user', @CurrentUser, 'table', 'AhphReRoute', 'column', 'DelegatingHandlers'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '路由优先级,多个路由匹配上，优先级高的先执行',
   'user', @CurrentUser, 'table', 'AhphReRoute', 'column', 'Priority'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '当前状态, 1 有效 0 无效',
   'user', @CurrentUser, 'table', 'AhphReRoute', 'column', 'InfoStatus'
go

/*==============================================================*/
/* Index: 分类路由信息_FK                                             */
/*==============================================================*/
create index 分类路由信息_FK on AhphReRoute (
ItemId ASC
)
go

/*==============================================================*/
/* Table: AhphReRoutesItem                                      */
/*==============================================================*/
create table AhphReRoutesItem (
   ItemId               int                  identity,
   ItemName             varchar(100)         not null,
   ItemDetail           varchar(500)         null,
   ItemParentId         int                  null,
   InfoStatus           int                  not null default 1,
   constraint PK_AHPHREROUTESITEM primary key nonclustered (ItemId)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '路由分类表',
   'user', @CurrentUser, 'table', 'AhphReRoutesItem'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '分类主键',
   'user', @CurrentUser, 'table', 'AhphReRoutesItem', 'column', 'ItemId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '分类名称',
   'user', @CurrentUser, 'table', 'AhphReRoutesItem', 'column', 'ItemName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '分类描述',
   'user', @CurrentUser, 'table', 'AhphReRoutesItem', 'column', 'ItemDetail'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '上级分类,顶级节点为空',
   'user', @CurrentUser, 'table', 'AhphReRoutesItem', 'column', 'ItemParentId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '当前状态, 1 有效 0 无效',
   'user', @CurrentUser, 'table', 'AhphReRoutesItem', 'column', 'InfoStatus'
go

