using AutoMapper;

namespace IdentityServer4.Dapper.Mappers
{
    /// <summary>
    /// 金焰的世界
    /// 2018-12-03
    /// 客户端信息映射
    /// </summary>
    public static class ClientMappers
    {
        static ClientMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ClientMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static Models.Client ToModel(this Entities.Client entity)
        {
            return Mapper.Map<Models.Client>(entity);
        }

        public static Entities.Client ToEntity(this Models.Client model)
        {
            return Mapper.Map<Entities.Client>(model);
        }
    }
}