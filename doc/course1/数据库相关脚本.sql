--插入全局测试信息
insert into AhphGlobalConfiguration(GatewayName,RequestIdKey,IsDefault,InfoStatus)
values('测试网关','test_gateway',1,1);

--插入路由分类测试信息
insert into AhphReRoutesItem(ItemName,InfoStatus) values('测试分类',1);

--插入路由测试信息 
insert into AhphReRoute values(1,'/ctr/values','[ "GET" ]','','http','/api/Values','[{"Host": "localhost","Port": 9000 }]',
'','','','','','','',0,1);

--插入网关关联表
insert into dbo.AhphConfigReRoutes values(1,1);
