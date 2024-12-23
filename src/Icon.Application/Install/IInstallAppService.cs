using System.Threading.Tasks;
using Abp.Application.Services;
using Icon.Install.Dto;

namespace Icon.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}