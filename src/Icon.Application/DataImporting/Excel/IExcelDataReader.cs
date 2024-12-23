﻿using System.Collections.Generic;
using Abp.Dependency;

namespace Icon.DataImporting.Excel;

public interface IExcelDataReader<TEntityDto> : ITransientDependency
{
    List<TEntityDto> GetEntitiesFromExcel(byte[] fileBytes);
}