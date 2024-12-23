using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.OpenIddict.EntityFrameworkCore.Applications;
using Icon.EntityFrameworkCore;

namespace Icon.OpenIddict.Applications
{
    public class OpenIddictApplicationRepository : EfCoreOpenIddictApplicationRepository<IconDbContext>
    {
        public OpenIddictApplicationRepository(
            IDbContextProvider<IconDbContext> dbContextProvider,
            IUnitOfWorkManager unitOfWorkManager) : base(dbContextProvider, unitOfWorkManager)
        {
        }
    }
}