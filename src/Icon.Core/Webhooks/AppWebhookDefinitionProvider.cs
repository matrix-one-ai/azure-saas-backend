using Abp.Localization;
using Abp.Webhooks;
using Icon.Webhooks;

namespace Icon.WebHooks
{
    public class AppWebhookDefinitionProvider : WebhookDefinitionProvider
    {
        public override void SetWebhooks(IWebhookDefinitionContext context)
        {
            context.Manager.Add(new WebhookDefinition(
                name: AppWebHookNames.TestWebhook
            ));

            //Add your webhook definitions here 
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, IconConsts.LocalizationSourceName);
        }
    }
}
