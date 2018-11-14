
--插入路由测试信息 
insert into AhphReRoute values(1,'/ctr/values/{id}','[ "GET" ]','','http','/api/Values/{id}','[{"Host": "localhost","Port": 9000 }]',
'','','"FileCacheOptions": { "TtlSeconds": 60, "Region": "test_ahphocelot" }','','','','',0,1);

--插入网关关联表
insert into dbo.AhphConfigReRoutes values(1,2);
