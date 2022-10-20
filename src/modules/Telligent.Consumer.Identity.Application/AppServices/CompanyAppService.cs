using AutoMapper;
using Microsoft.AspNetCore.Http;
using Telligent.Consumer.Identity.Application.Dtos.Company;
using Telligent.Consumer.Identity.Application.Dtos.Corporation;
using Telligent.Consumer.Identity.Application.Dtos.Tenant;
using Telligent.Consumer.Identity.Domain.Organizations;
using Telligent.Core.Application.Services;
using Telligent.Core.Domain.Repositories;

namespace Telligent.Consumer.Identity.Application.AppServices;

public class CompanyAppService : CrudAppService<Company, CompanyDto, CompanyDto, CompanyDto>
{
    private readonly IRepository<Tenant> _tenantRepository;
    private readonly IRepository<Corporation> _corporationRepository;

    public CompanyAppService(
        IRepository<Tenant> tenantRepository,
        IRepository<Corporation> corporationRepository,
        IRepository<Company> repository,
        IMapper mapper, 
        IHttpContextAccessor httpContextAccessor) : base(repository, mapper, httpContextAccessor)
    {
        _tenantRepository = tenantRepository;
        _corporationRepository = corporationRepository;
    }

    /// <summary>
    /// 取得公司資訊
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public override async Task<CompanyDto> GetAsync(Guid id)
    {
        var company = await Repository.GetAsync(id);

        if(company == null)
            throw new InvalidOperationException("The company cannot be found.");

        var companyDto = Mapper.Map<CompanyDto>(company);
        var tenantDto = Mapper.Map<TenantDto>(await _tenantRepository.GetAsync(company.TenantId));
        var corporationDto = Mapper.Map<CorporationDto>(await _corporationRepository.GetAsync(company.CorporationId));

        if(tenantDto == null)
            throw new InvalidOperationException("The company's tenant cannot be found.");

        if(corporationDto == null)
            throw new InvalidOperationException("The company's corporation cannot be found.");

        companyDto.Tenant = tenantDto;
        companyDto.Corporation = corporationDto;

        return companyDto;
    }
}