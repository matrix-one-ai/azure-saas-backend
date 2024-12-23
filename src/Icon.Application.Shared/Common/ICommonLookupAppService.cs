using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Icon.Common.Dto;
using Icon.Editions.Dto;

namespace Icon.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        Task<PagedResultDto<FindUsersOutputDto>> FindUsers(FindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();
    }
}