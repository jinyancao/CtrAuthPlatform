// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using AutoMapper;
namespace IdentityServer4.Dapper.Mappers
{
    /// <summary>
    /// Defines entity/model mapping for API resources.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class ApiResourceMapperProfile : Profile
    {
        /// <summary>
        /// <see cref="ApiResourceMapperProfile"/>
        /// </summary>
        public ApiResourceMapperProfile()
        {
            CreateMap<Entities.ApiResource, IdentityServer4.Models.ApiResource>(MemberList.Destination)
                .ConstructUsing(src => new IdentityServer4.Models.ApiResource())
                .ForMember(x => x.ApiSecrets, opts => opts.MapFrom(x => x.Secrets))
                .ReverseMap();

            CreateMap<Entities.ApiResourceClaim, string>()
                .ConstructUsing(x => x.Type)
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

            CreateMap<Entities.ApiSecret, IdentityServer4.Models.Secret>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ReverseMap();

            CreateMap<Entities.ApiScope, IdentityServer4.Models.Scope>(MemberList.Destination)
                .ConstructUsing(src => new IdentityServer4.Models.Scope())
                .ReverseMap();

            CreateMap<Entities.ApiScopeClaim, string>()
               .ConstructUsing(x => x.Type)
               .ReverseMap()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));
        }
    }
}
