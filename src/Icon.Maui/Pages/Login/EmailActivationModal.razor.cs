using Microsoft.AspNetCore.Components;
using Icon.Authorization.Accounts;
using Icon.Authorization.Accounts.Dto;
using Icon.Maui.Core;
using Icon.Maui.Core.Components;
using Icon.Maui.Core.Threading;
using Icon.Maui.Models.Login;

namespace Icon.Maui.Pages.Login
{
    public partial class EmailActivationModal : ModalBase
    {
        public override string ModalId => "email-activation-modal";

        [Parameter] public EventCallback OnSave { get; set; }

        public EmailActivationModel emailActivationModel { get; set; } = new EmailActivationModel();

        private readonly IAccountAppService _accountAppService;

        public EmailActivationModal()
        {
            _accountAppService = Resolve<IAccountAppService>();
        }

        protected virtual async Task Save()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestExecuter.Execute(
                async () =>
                    await _accountAppService.SendEmailActivationLink(new SendEmailActivationLinkInput
                    {
                        EmailAddress = emailActivationModel.EmailAddress
                    }),
                    async () =>
                    {
                        await OnSave.InvokeAsync();
                    }
                );
            });
        }

        protected virtual async Task Cancel()
        {
            await Hide();
        }
    }
}
