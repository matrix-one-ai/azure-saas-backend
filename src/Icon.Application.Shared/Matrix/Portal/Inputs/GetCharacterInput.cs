using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.Web.Models.AbpUserConfiguration;
using Icon.Dto;

public class GetCharacterInput
{
    public Guid? CharacterId { get; set; }
    public string CharacterName { get; set; }
    public string PlatformName { get; set; }

    public bool IncludeCharacterBios { get; set; } = false;
    public bool IncludePersonas { get; set; } = false;
}
