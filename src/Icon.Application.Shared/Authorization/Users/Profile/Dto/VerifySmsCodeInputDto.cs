namespace Icon.Authorization.Users.Profile.Dto
{
    public class VerifySmsCodeInputDto
    {
        public string Code { get; set; }

        public string PhoneNumber { get; set; }
    }
}