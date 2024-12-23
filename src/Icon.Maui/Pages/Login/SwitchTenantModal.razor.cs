﻿using Microsoft.AspNetCore.Components;
using Icon.Maui.Core.Components;

namespace Icon.Maui.Pages.Login
{
    public partial class SwitchTenantModal : ModalBase
    {
        public override string ModalId => "switch-tenant";

        [Parameter] public EventCallback<string> OnSave { get; set; }

        protected string TenancyName { get; set; }

        protected virtual async Task Save()
        {
            await Hide();
            await OnSave.InvokeAsync(TenancyName);
            TenancyName = null;
        }

        protected virtual async Task Cancel()
        {
            TenancyName = null;
            await Hide();
        }
    }
}
