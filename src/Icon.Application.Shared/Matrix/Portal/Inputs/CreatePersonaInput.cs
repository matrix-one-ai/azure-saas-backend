// Create character bio input
using System;
using Abp.Application.Services.Dto;

public class CreatePersonaInput
{
    public int TenantId { get; set; }
    public string PersonaName { get; set; }
    public string PlatformName { get; set; }
    public string CharacterName { get; set; }
    public string Attitude { get; set; }
    public string Repsonses { get; set; }
    public bool RespondNewPosts { get; set; }
    public bool RespondReplies { get; set; }
    public bool PersonaIsAi { get; set; }
}
