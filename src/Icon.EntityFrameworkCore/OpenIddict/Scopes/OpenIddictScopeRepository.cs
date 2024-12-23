using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.OpenIddict.EntityFrameworkCore.Scopes;
using Icon.EntityFrameworkCore;

namespace Icon.OpenIddict.Scopes
{
    public class OpenIddictScopeRepository : EfCoreOpenIddictScopeRepository<IconDbContext>
    {
        public OpenIddictScopeRepository(
            IDbContextProvider<IconDbContext> dbContextProvider,
            IUnitOfWorkManager unitOfWorkManager) : base(dbContextProvider, unitOfWorkManager)
        {
        }
    }
}