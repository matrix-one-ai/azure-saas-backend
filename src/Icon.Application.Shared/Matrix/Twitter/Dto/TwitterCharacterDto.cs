using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Icon.BaseManagement;


namespace Icon.Matrix.Twitter.Dto
{
    public class TwitterCharacterDto
    {
        public Guid CharacterId { get; set; }
        public string CharacterName { get; set; }
        public string PlatformCharacterId { get; set; }
    }
}
