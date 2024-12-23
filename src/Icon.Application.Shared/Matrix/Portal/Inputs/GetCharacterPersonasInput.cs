using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.Web.Models.AbpUserConfiguration;
using Icon.Dto;

public class GetCharacterPersonasInput : PagedAndSortedInputDto, IShouldNormalize
{
    public Guid? CharacterId { get; set; }
    public string CharacterName { get; set; }
    public string PlatformName { get; set; }
    public string PersonaName { get; set; }
    public void Normalize()
    {
        if (Sorting.IsNullOrWhiteSpace())
        {
            Sorting = "Persona.Name";
        }
    }
}

