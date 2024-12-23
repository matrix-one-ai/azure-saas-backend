using System.Collections.Generic;
using Abp.Collections.Extensions;
using Icon.Authorization.Users.Importing.Dto;
using Icon.DataExporting.Excel.MiniExcel;
using Icon.DataImporting.Excel;
using Icon.Dto;
using Icon.Storage;

namespace Icon.Authorization.Users.Importing
{
    public class InvalidUserExporter(ITempFileCacheManager tempFileCacheManager)
        : MiniExcelExcelExporterBase(tempFileCacheManager), IExcelInvalidEntityExporter<ImportUserDto>
    {
        public FileDto ExportToFile(List<ImportUserDto> userList)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var user in userList)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("UserName"), user.UserName},
                    {L("Name"), user.Name},
                    {L("Surname"), user.Surname},
                    {L("EmailAddress"), user.EmailAddress},
                    {L("PhoneNumber"), user.PhoneNumber},
                    {L("Password"), user.Password},
                    {L("Roles"), user.Roles?.JoinAsString(",")},
                    {L("RefuseReason"), user.Exception}
                });
            }

            return CreateExcelPackage("InvalidUserImportList.xlsx", items);
        }
    }
}
