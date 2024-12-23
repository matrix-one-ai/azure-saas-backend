using System.ComponentModel.DataAnnotations;
using Icon.Validation;

namespace Icon.Maui.Models.Login
{
    public class EmailActivationModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
