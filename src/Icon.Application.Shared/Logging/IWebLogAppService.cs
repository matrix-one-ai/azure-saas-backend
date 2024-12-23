using Abp.Application.Services;
using Icon.Dto;
using Icon.Logging.Dto;

namespace Icon.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
