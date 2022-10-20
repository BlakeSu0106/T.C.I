using AutoMapper;
using Microsoft.AspNetCore.Http;
using Telligent.Consumer.Identity.Application.Dtos.Member;
using Telligent.Consumer.Identity.Domain.Members;
using Telligent.Core.Application.Services;
using Telligent.Core.Domain.Repositories;
using Telligent.Core.Infrastructure.Encryption;
using Telligent.Core.Infrastructure.Extensions;
using Telligent.Core.Infrastructure.Generators;

namespace Telligent.Consumer.Identity.Application.AppServices;

public class MemberAppService : CrudAppService<Member, MemberDto, CreateMemberDto, UpdateMemberDto>
{
    private readonly CompanyAppService _companyAppService;

    public MemberAppService(
        IRepository<Member> repository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        CompanyAppService companyAppService) : base(repository, mapper, httpContextAccessor)
    {
        _companyAppService = companyAppService;
    }

    public override async Task<MemberDto> CreateAsync(CreateMemberDto dto)
    {
        var company = await _companyAppService.GetAsync(dto.CompanyId);
        var entity = Mapper.Map<Member>(dto);

        entity.Id = SequentialGuidGenerator.Instance.GetGuid();
        entity.TenantId = company.Tenant.Id;
        entity.CorporationId = company.Corporation.Id;
        entity.Password = EncryptionHelper.EncryptSha1(dto.Password);
        entity.RegistrationTime = DateTime.Now.ToUtc8DateTime();
        entity.CreatorId = entity.Id;

        await Repository.CreateAsync(entity);
        await Repository.SaveChangesAsync();

        return await GetAsync(entity.Id);
    }

    public async Task<MemberDto> GetAsync(string companyId, string userId, string password)
    {
        return await GetAsync(m =>
            m.CompanyId.Equals(Guid.Parse(companyId)) &&
            m.UserId.Equals(userId) &&
            m.Password.Equals(EncryptionHelper.EncryptSha1(password)) &&
            m.EntityStatus);
    }
}