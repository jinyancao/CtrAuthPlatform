--1、插入限流规则
INSERT INTO AhphLimitRule VALUES('每1分钟访问1次','1m',1,1);
INSERT INTO AhphLimitRule VALUES('每1分钟访问60次','1m',60,1);

--2、应用到/cjy/values路由
INSERT INTO AhphReRouteLimitRule VALUES(1,1);
INSERT INTO AhphReRouteLimitRule VALUES(2,1);

--3、插入测试分组
INSERT INTO AhphLimitGroup VALUES('限流分组1','',1);
INSERT INTO AhphLimitGroup VALUES('限流分组2','',1);
--4、分组应用策略
INSERT INTO AhphLimitGroupRule VALUES(1,1);
INSERT INTO AhphLimitGroupRule VALUES(2,2);

--5、客户端应用限流分组
INSERT INTO AhphClientLimitGroup VALUES(2,1);
INSERT INTO AhphClientLimitGroup VALUES(3,2);

--6、设置客户端1/cjy/values路由白名单
INSERT INTO AhphClientReRouteWhiteList VALUES(1,2);