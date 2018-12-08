/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2018/11/12 22:50:27                          */
/*==============================================================*/


drop table if exists AhphConfigReRoutes;

drop table if exists AhphGlobalConfiguration;

drop table if exists AhphReRoute;

drop table if exists AhphReRoutesItem;

/*==============================================================*/
/* Table: AhphConfigReRoutes                                    */
/*==============================================================*/
create table AhphConfigReRoutes
(
   CtgRouteId           int not null auto_increment comment '配置路由主键',
   AhphId               int comment '网关主键',
   ReRouteId            int comment '路由主键',
   primary key (CtgRouteId)
);

alter table AhphConfigReRoutes comment '网关-路由,可以配置多个网关和多个路由';

/*==============================================================*/
/* Table: AhphGlobalConfiguration                               */
/*==============================================================*/
create table AhphGlobalConfiguration
(
   AhphId               int not null auto_increment comment '网关主键',
   GatewayName          varchar(100) not null comment '网关名称',
   RequestIdKey         varchar(100) comment '全局请求默认key',
   BaseUrl              varchar(100) comment '请求路由根地址',
   DownstreamScheme     varchar(50) comment '下游使用架构',
   ServiceDiscoveryProvider varchar(500) comment '服务发现全局配置,值为配置json',
   LoadBalancerOptions  varchar(500) comment '全局负载均衡配置',
   HttpHandlerOptions   varchar(500) comment 'Http请求配置',
   QoSOptions           varchar(200) comment '请求安全配置,超时、重试、熔断',
   IsDefault            int not null default 0 comment '是否默认配置, 1 默认 0 默认',
   InfoStatus           int not null default 1 comment '当前状态, 1 有效 0 无效',
   primary key (AhphId)
);

alter table AhphGlobalConfiguration comment '网关全局配置表';

/*==============================================================*/
/* Table: AhphReRoute                                           */
/*==============================================================*/
create table AhphReRoute
(
   ReRouteId            int not null auto_increment comment '路由主键',
   ItemId               int comment '分类主键',
   UpstreamPathTemplate varchar(150) not null comment '上游路径模板，支持正则',
   UpstreamHttpMethod   varchar(50) not null comment '上游请求方法数组格式',
   UpstreamHost         varchar(100) comment '上游域名地址',
   DownstreamScheme     varchar(50) not null comment '下游使用架构',
   DownstreamPathTemplate varchar(200) not null comment '下游路径模板,与上游正则对应',
   DownstreamHostAndPorts varchar(500) comment '下游请求地址和端口,静态负载配置',
   AuthenticationOptions varchar(300) comment '授权配置,是否需要认证访问',
   RequestIdKey         varchar(100) comment '全局请求默认key',
   CacheOptions         varchar(200) comment '缓存配置,常用查询和再次配置缓存',
   ServiceName          varchar(100) comment '服务发现名称,启用服务发现时生效',
   LoadBalancerOptions  varchar(500) comment '全局负载均衡配置',
   QoSOptions           varchar(200) comment '请求安全配置,超时、重试、熔断',
   DelegatingHandlers   varchar(200) comment '委托处理方法,特定路由定义委托单独处理',
   Priority             int comment '路由优先级,多个路由匹配上，优先级高的先执行',
   InfoStatus           int not null default 1 comment '当前状态, 1 有效 0 无效',
   primary key (ReRouteId)
);

alter table AhphReRoute comment '路由配置表';

/*==============================================================*/
/* Table: AhphReRoutesItem                                      */
/*==============================================================*/
create table AhphReRoutesItem
(
   ItemId               int not null auto_increment comment '分类主键',
   ItemName             varchar(100) not null comment '分类名称',
   ItemDetail           varchar(500) comment '分类描述',
   ItemParentId         int comment '上级分类,顶级节点为空',
   InfoStatus           int not null default 1 comment '当前状态, 1 有效 0 无效',
   primary key (ItemId)
);

alter table AhphReRoutesItem comment '路由分类表';

alter table AhphConfigReRoutes add constraint FK_Relationship_4 foreign key (AhphId)
      references AhphGlobalConfiguration (AhphId) on delete restrict on update restrict;

alter table AhphConfigReRoutes add constraint FK_Relationship_5 foreign key (ReRouteId)
      references AhphReRoute (ReRouteId) on delete restrict on update restrict;

alter table AhphReRoute add constraint FK_分类路由信息 foreign key (ItemId)
      references AhphReRoutesItem (ItemId) on delete restrict on update restrict;

