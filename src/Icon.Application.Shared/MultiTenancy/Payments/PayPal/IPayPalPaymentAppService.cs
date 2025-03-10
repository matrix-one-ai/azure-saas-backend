﻿using System.Threading.Tasks;
using Abp.Application.Services;
using Icon.MultiTenancy.Payments.PayPal.Dto;

namespace Icon.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
