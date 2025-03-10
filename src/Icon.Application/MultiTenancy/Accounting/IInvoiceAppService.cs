﻿using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Icon.MultiTenancy.Accounting.Dto;

namespace Icon.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
