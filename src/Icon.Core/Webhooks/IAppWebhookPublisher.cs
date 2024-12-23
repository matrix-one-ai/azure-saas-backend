using System.Threading.Tasks;
using Icon.Authorization.Users;

namespace Icon.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
