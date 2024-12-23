using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Icon.Authorization.Permissions.Dto;

namespace Icon.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
