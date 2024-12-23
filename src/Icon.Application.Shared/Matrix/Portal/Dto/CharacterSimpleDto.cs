using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
namespace Icon.Matrix.Portal.Dto
{
    public class CharacterSimpleDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}