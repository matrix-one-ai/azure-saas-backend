using System.Threading.Tasks;
using Abp.Webhooks;

namespace Icon.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
