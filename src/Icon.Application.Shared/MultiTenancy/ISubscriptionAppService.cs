using System.Threading.Tasks;
using Abp.Application.Services;
using Icon.MultiTenancy.Dto;
using Icon.MultiTenancy.Payments.Dto;

namespace Icon.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
        
        Task<long> StartExtendSubscription(StartExtendSubscriptionInput input);
        
        Task<StartUpgradeSubscriptionOutput> StartUpgradeSubscription(StartUpgradeSubscriptionInput input);
        
        Task<long> StartTrialToBuySubscription(StartTrialToBuySubscriptionInput input);
    }
}
