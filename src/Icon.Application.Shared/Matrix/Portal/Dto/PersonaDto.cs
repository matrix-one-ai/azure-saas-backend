using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Icon.BaseManagement;
namespace Icon.Matrix.Portal.Dto
{
    public class PersonaDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public IList<PersonaPlatformDto> Platforms { get; set; }
        public string PlatformNames { get; set; }
    }

    public class PersonaPlatformDto : EntityDto<Guid>
    {
        public string PlatformName { get; set; }
        public string PlatformPersonaId { get; set; }
    }
}

