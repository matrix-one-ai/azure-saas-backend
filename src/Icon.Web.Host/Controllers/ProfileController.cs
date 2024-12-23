using Abp.AspNetCore.Mvc.Authorization;
using Icon.Authorization.Users.Profile;
using Icon.Graphics;
using Icon.Storage;

namespace Icon.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService,
            IImageValidator imageValidator) :
            base(tempFileCacheManager, profileAppService, imageValidator)
        {
        }
    }
}