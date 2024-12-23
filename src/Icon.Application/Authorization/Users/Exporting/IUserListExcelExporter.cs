using System.Collections.Generic;
using Icon.Authorization.Users.Dto;
using Icon.Dto;

namespace Icon.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos, List<string> selectedColumns);
    }
}