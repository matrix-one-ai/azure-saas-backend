using System;
using Abp.Application.Services.Dto;
namespace Icon.Matrix.Portal.Dto
{
    public class PlatformDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
