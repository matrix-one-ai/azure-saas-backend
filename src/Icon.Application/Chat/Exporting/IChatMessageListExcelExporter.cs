using System.Collections.Generic;
using Abp;
using Icon.Chat.Dto;
using Icon.Dto;

namespace Icon.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
