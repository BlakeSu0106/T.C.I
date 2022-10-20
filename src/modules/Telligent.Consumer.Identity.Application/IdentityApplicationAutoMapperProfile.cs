using AutoMapper;
using Telligent.Consumer.Identity.Application.Dtos.Company;
using Telligent.Consumer.Identity.Application.Dtos.Corporation;
using Telligent.Consumer.Identity.Application.Dtos.Member;
using Telligent.Consumer.Identity.Application.Dtos.Tenant;
using Telligent.Consumer.Identity.Domain.Members;
using Telligent.Consumer.Identity.Domain.Organizations;

namespace Telligent.Consumer.Identity.Application;

public class IdentityApplicationAutoMapperProfile : Profile
{
    public IdentityApplicationAutoMapperProfile()
    {
        ShouldMapProperty = prop =>
            prop.GetMethod is not null && (prop.GetMethod.IsAssembly || prop.GetMethod.IsPublic);

        CreateMap<Tenant, TenantDto>();
        CreateMap<Corporation, CorporationDto>();
        CreateMap<Company, CompanyDto>();

        CreateMap<CreateMemberDto, Member>();
        CreateMap<Member, MemberDto>();
    }
}