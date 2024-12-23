using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.Web.Models.AbpUserConfiguration;
using Icon.Dto;

public class GetCharactersInput : PagedAndSortedInputDto, IShouldNormalize
{
    public Guid? CharacterId { get; set; }
    public string CharacterName { get; set; }
    public void Normalize()
    {
        if (Sorting.IsNullOrWhiteSpace())
        {
            Sorting = "Name ASC";
        }
    }
}

