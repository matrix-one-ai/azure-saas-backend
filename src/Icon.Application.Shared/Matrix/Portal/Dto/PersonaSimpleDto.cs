using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Icon.BaseManagement;
namespace Icon.Matrix.Portal.Dto
{
    public class PersonaSimpleDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}

