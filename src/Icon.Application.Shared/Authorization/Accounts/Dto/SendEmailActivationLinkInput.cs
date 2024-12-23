using Abp.Auditing;
using System.ComponentModel.DataAnnotations;

namespace Icon.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }

        [DisableAuditing]
        public string CaptchaResponse { get; set; }
    }
}