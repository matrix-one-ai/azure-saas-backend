using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Authorization;
using Icon.BaseManagement;
using Abp.UI;

namespace Icon.BaseManagement
{
    [AbpAuthorize]
    public class BaseModalAppService : IconAppServiceBase
    {
        private readonly IPermissionChecker _permissionChecker;

        public BaseModalAppService(
            IPermissionChecker permissionChecker)
        {
            _permissionChecker = permissionChecker;
        }

        [HttpGet]
        public async Task<OpenBaseModalInput> OpenBaseModal(OpenBaseModalInput input)
        {
            await Task.CompletedTask;
            return new OpenBaseModalInput();
        }
    }
}