using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Icon.BaseManagement;
namespace Icon.Matrix.Portal.Dto
{
    public class PersonaTwitterDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string TwitterHandle { get; set; }
        public string TwitterAvatarUrl { get; set; }
    }
}

