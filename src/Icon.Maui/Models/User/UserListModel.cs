using Abp.AutoMapper;
using Icon.Authorization.Users.Dto;

namespace Icon.Maui.Models.User
{
    [AutoMapFrom(typeof(UserListDto))]
    public class UserListModel : UserListDto
    {
        public string Photo { get; set; }

        public string FullName => Name + " " + Surname;
    }
}
