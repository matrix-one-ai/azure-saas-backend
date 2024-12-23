using System;
using Abp.Application.Services.Dto;
namespace Icon.Matrix.Portal.Dto
{
    public class MemoryTypeDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }

}