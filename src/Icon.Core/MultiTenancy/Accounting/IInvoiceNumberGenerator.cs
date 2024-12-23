using System.Threading.Tasks;
using Abp.Dependency;

namespace Icon.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}