using System.ComponentModel.DataAnnotations;

namespace Icon.Maui.Models.Login
{
    public class ForgotPasswordModel
    {
        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; }
    }
}
