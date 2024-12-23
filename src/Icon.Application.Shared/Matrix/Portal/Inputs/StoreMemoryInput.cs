using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Web.Models.AbpUserConfiguration;

public class StoreMemoryInput
{
    [Required]
    public string CharacterName { get; set; }

    [Required]
    public string PlatformName { get; set; }
    public string PlatformPersonaId { get; set; }
    public string PlatformPersonaName { get; set; }
    public string PlatformInteractionId { get; set; }
    public string PlatformInteractionParentId { get; set; }
    public DateTimeOffset? PlatformInteractionDate { get; set; }


    [Required]
    public string MemoryType { get; set; }
    public string MemoryTitle { get; set; }
    [Required]
    public string MemoryContent { get; set; }

}
