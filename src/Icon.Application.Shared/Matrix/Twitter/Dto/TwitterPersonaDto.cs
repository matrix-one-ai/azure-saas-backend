using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Icon.BaseManagement;


namespace Icon.Matrix.Twitter.Dto
{
    public class TwitterPersonaDto
    {
        public Guid CharacterId { get; set; }
        public string CharacterName { get; set; }
        public Guid PersonaId { get; set; }
        public string PlatformPersonaName { get; set; }
        public string PlatformPersonaId { get; set; }
        public bool DownloadNewPosts { get; set; } = true;
    }
}
