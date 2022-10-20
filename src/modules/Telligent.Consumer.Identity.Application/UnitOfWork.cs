using Telligent.Consumer.Identity.Domain.Members;
using Telligent.Consumer.Identity.Domain.Organizations;
using Telligent.Core.Domain.Repositories;
using Telligent.Core.Infrastructure.Database;

namespace Telligent.Consumer.Identity.Application;

public class UnitOfWork : IDisposable
{
    private bool _disposed;

    public UnitOfWork(
        BaseDbContext context,
        IRepository<Tenant> tenantRepository,
        IRepository<Company> companyRepository,
        IRepository<Member> memberRepository,
        IRepository<MemberCaptcha> memberCaptchaRepository)
    {
        Context = context;
        TenantRepository = tenantRepository;
        CompanyRepository = companyRepository;
        MemberRepository = memberRepository;
        MemberCaptchaRepository = memberCaptchaRepository;
    }

    public IRepository<Tenant> TenantRepository { get; }
    public IRepository<Company> CompanyRepository { get; }
    public IRepository<Member> MemberRepository { get; set; }
    public IRepository<MemberCaptcha> MemberCaptchaRepository { get; }

    /// <summary>
    /// Context
    /// </summary>
    public BaseDbContext Context { get; private set; }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// SaveChange
    /// </summary>
    /// <returns></returns>
    public async Task<int> SaveChangeAsync()
    {
        return await Context.SaveChangesAsync();
    }

    /// <summary>
    /// Dispose
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
            {
                Context.Dispose();
                Context = null;
            }

        _disposed = true;
    }
}