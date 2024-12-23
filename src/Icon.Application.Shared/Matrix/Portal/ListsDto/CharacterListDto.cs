using System;
using Abp.Application.Services.Dto;
using Icon.BaseManagement;

namespace Icon.Matrix.Portal.Dto
{
    public class CharacterListDto : EntityDto<Guid>
    {
        public CharacterDto Character { get; set; }
        public BaseListRowSettingsDto RowSettings { get; set; }
    }
}