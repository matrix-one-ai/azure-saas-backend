using Icon.Auditing.Dto;
using Icon.Dto;
using Icon.EntityChanges.Dto;
using System.Collections.Generic;

namespace Icon.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
