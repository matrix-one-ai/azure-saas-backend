using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.Web.Models.AbpUserConfiguration;
using Icon.Dto;

namespace Icon.Matrix.Twitter.Inputs
{
    public class GetTwitterCharacterPersonasInput
    {
        public Guid? CharacterId { get; set; }
        public string CharacterName { get; set; }
    }
}