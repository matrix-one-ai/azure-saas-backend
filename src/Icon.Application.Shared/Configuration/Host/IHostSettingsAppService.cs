using System.Threading.Tasks;
using Abp.Application.Services;
using Icon.Configuration.Host.Dto;

namespace Icon.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
