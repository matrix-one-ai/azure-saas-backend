namespace Icon.Matrix.Portal.Dto
{
    public class CharacterPersonaRankListDto
    {
        public PersonaTwitterDto Persona { get; set; }
        public CharacterPersonaTwitterRankDto TwitterRank { get; set; }
        public int GardnerLevel { get; set; }

        // public Guid Id { get; set; }
        // public CharacterSimpleDto Character { get; set; }
        // public string Attitude { get; set; }
        // public string Responses { get; set; }
        // public bool ShouldRespondNewPosts { get; set; }
        // public bool ShouldRespondMentions { get; set; }
        // public bool ShouldImportNewPosts { get; set; }
        // public BaseListRowSettingsDto RowSettings { get; set; }
    }
}