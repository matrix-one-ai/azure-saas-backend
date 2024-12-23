using System.Collections.Generic;
using Abp.Dependency;
using Icon.Dto;

namespace Icon.DataImporting.Excel;

public interface IExcelInvalidEntityExporter<TEntityDto> : ITransientDependency
{
    FileDto ExportToFile(List<TEntityDto> entities);
}