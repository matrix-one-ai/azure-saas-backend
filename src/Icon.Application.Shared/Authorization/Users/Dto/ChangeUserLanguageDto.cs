using System.ComponentModel.DataAnnotations;

namespace Icon.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
