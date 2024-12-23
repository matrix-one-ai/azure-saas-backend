using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Icon.MultiTenancy.HostDashboard.Dto;

namespace Icon.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}