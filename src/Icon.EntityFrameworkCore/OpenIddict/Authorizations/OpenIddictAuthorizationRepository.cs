using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.OpenIddict.EntityFrameworkCore.Authorizations;
using Icon.EntityFrameworkCore;

namespace Icon.OpenIddict.Authorizations
{
    public class OpenIddictAuthorizationRepository : EfCoreOpenIddictAuthorizationRepository<IconDbContext>
    {
        public OpenIddictAuthorizationRepository(
            IDbContextProvider<IconDbContext> dbContextProvider,
            IUnitOfWorkManager unitOfWorkManager) : base(dbContextProvider, unitOfWorkManager)
        {
        }
    }
}