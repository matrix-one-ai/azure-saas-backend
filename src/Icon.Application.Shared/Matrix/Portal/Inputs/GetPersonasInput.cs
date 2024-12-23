using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.Web.Models.AbpUserConfiguration;
using Icon.Dto;

public class GetPersonasInput : PagedAndSortedInputDto, IShouldNormalize
{
    public Guid? PersonaId { get; set; }
    public string PersonaNameFilter { get; set; }
    public DateTime? DateTimeStart { get; set; }
    public DateTime? DateTimeEnd { get; set; }
    public void Normalize()
    {
        if (Sorting.IsNullOrWhiteSpace())
        {
            Sorting = "CreatedAt DESC";
        }
    }

}
