using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.OpenIddict.EntityFrameworkCore.Tokens;
using Icon.EntityFrameworkCore;

namespace Icon.OpenIddict.Tokens
{
    public class OpenIddictTokenRepository : EfCoreOpenIddictTokenRepository<IconDbContext>
    {
        public OpenIddictTokenRepository(
            IDbContextProvider<IconDbContext> dbContextProvider,
            IUnitOfWorkManager unitOfWorkManager) : base(dbContextProvider, unitOfWorkManager)
        {
        }
    }
}