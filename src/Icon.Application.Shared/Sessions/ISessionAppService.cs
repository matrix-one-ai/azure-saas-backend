using System.Threading.Tasks;
using Abp.Application.Services;
using Icon.Sessions.Dto;

namespace Icon.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
