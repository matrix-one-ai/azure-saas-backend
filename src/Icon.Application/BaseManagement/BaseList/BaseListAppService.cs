using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Authorization;
using Icon.BaseManagement;

namespace Icon.BaseManagement
{
    [AbpAuthorize]
    public class BaseListAppService : IconAppServiceBase
    {
        private readonly IPermissionChecker _permissionChecker;

        public BaseListAppService(
            IPermissionChecker permissionChecker)
        {
            _permissionChecker = permissionChecker;
        }

        [HttpPost]
        public async Task<BaseModalDto> GetBasePage(GetBaseModalInput input)
        {
            await Task.CompletedTask;
            return null;
            //return new BaseModalBuilder().Build(input.ModalType, input.EntityId, input.LocationId);
        }

        [HttpGet]
        public async Task<OpenBaseModalInput> OpenBaseModal(OpenBaseModalInput input)
        {
            await Task.CompletedTask;
            return new OpenBaseModalInput();
        }
    }
}