using Abp.Application.Services.Dto;
using Abp.ObjectMapping;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Icon.Common;
using Icon.Editions.Dto;
using Icon.Maui.Core;
using Icon.Maui.Core.Components;
using Icon.Maui.Core.Extensions;
using Icon.Maui.Core.Threading;
using Icon.Maui.Core.Validations;
using Icon.Maui.Models.Tenants;
using Icon.MultiTenancy;
using Icon.MultiTenancy.Dto;

namespace Icon.Maui.Pages.Tenant
{
    public partial class EditTenantModal : ModalBase
    {
        [Parameter] public EventCallback OnSaveCompleted { get; set; }

        public override string ModalId => "edit-tenant-modal";
        
        private ITenantAppService _tenantAppService;

        private readonly ICommonLookupAppService _commonLookupAppService;

        private EditTenantModel EditTenantModel { get; set; } = new();
        
        public EditTenantModal()
        {
            _tenantAppService = Resolve<ITenantAppService>();
            _commonLookupAppService = Resolve<ICommonLookupAppService>();
        }

        public async Task OpenFor(EntityDto entityDto)
        {
            await SetBusyAsync(async () =>
            {
                EditTenantModel = ObjectMapper.Map<EditTenantModel>(await _tenantAppService.GetTenantForEdit(entityDto));
                EditTenantModel.IsUnlimitedTimeSubscription = EditTenantModel.SubscriptionEndDateUtc == null;
                await PopulateEditionsCombobox();
                await Show();
            });
        }
        
        private async Task UpdateTenantAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestExecuter.Execute(async () =>
                {
                    EditTenantModel.NormalizeEditTenantInputModel();
                    var input = ObjectMapper.Map<TenantEditDto>(EditTenantModel);
                    
                    await _tenantAppService.UpdateTenant(input);

                }, async () =>
                {
                    await UserDialogsService.AlertSuccess(L("SuccessfullySaved"));
                    await Hide();
                    await OnSaveCompleted.InvokeAsync();
                });
            });

        }
        
        private async Task PopulateEditionsCombobox()
        {
            var editions = await _commonLookupAppService.GetEditionsForCombobox();
            EditTenantModel.Editions = editions.Items.ToList();

            EditTenantModel.Editions.Insert(0, new SubscribableEditionComboboxItemDto(EditTenantModel.NotAssignedValue,
                $"- {L("NotAssigned")} -", null));
            
            EditTenantModel.SelectedEdition = EditTenantModel.EditionId?.ToString() ?? EditTenantModel.NotAssignedValue;
        }

        public override Task Hide()
        {
            EditTenantModel = new EditTenantModel
            {
                Editions = []
            };
            return base.Hide();
        }
        
    }
}
