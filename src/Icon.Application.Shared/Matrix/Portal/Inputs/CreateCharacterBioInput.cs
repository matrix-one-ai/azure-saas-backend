// Create character bio input
using System;
using Abp.Application.Services.Dto;

public class CreateCharacterBioInput
{
    public int TenantId { get; set; }
    public Guid CharacterId { get; set; }
    public string Bio { get; set; }
    public string Personality { get; set; }
    public string Appearance { get; set; }
    public string Occupation { get; set; }
    public DateTimeOffset ActiveFrom { get; set; }
    public DateTimeOffset? ActiveTo { get; set; }
    public bool IsActive { get; set; }
}
