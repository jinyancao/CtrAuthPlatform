--插入测试客户端
INSERT INTO AhphClients(ClientId,ClientName) VALUES('client1','测试客户端1')
INSERT INTO AhphClients(ClientId,ClientName) VALUES('client2','测试客户端2')
--插入测试授权组
INSERT INTO AhphAuthGroup VALUES('授权组1','只能访问/cjy/values路由',1);
INSERT INTO AhphAuthGroup VALUES('授权组2','能访问所有路由',1);
--插入测试组权限
INSERT INTO AhphReRouteGroupAuth VALUES(1,1);

INSERT INTO AhphReRouteGroupAuth VALUES(2,1);
INSERT INTO AhphReRouteGroupAuth VALUES(2,2);

--插入客户端授权
INSERT INTO AhphClientGroup VALUES(3,1);
INSERT INTO AhphClientGroup VALUES(2,2);

--设置测试路由只有授权才能访问
UPDATE AhphReRoute SET AuthenticationOptions='{"AuthenticationProviderKey": "TestKey"}' WHERE ReRouteId IN(1,2);

SELECT * FROM dbo.AhphClients
