using System.Threading.Tasks;
using Abp.Application.Services;
using Icon.MultiTenancy.Payments.Dto;
using Icon.MultiTenancy.Payments.Stripe.Dto;

namespace Icon.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();
        
        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}