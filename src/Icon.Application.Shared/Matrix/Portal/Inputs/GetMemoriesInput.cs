using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.Web.Models.AbpUserConfiguration;
using Icon.Dto;

public class GetMemoriesInput : PagedAndSortedInputDto, IShouldNormalize
{
    public string MemoryContent { get; set; }
    public string MemoryCharacter { get; set; }
    public string MemoryPersona { get; set; }
    public Guid? MemoryTypeId { get; set; }
    public DateTime? DateTimeStart { get; set; }
    public DateTime? DateTimeEnd { get; set; }
    public void Normalize()
    {
        if (Sorting.IsNullOrWhiteSpace())
        {
            Sorting = "PlatformInteractionDate DESC";
        }
    }

}
