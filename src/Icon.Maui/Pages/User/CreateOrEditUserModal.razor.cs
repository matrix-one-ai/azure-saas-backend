using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Components;
using Icon.Authorization;
using Icon.Authorization.Users;
using Icon.Authorization.Users.Dto;
using Icon.Maui.Core;
using Icon.Maui.Core.Components;
using Icon.Maui.Core.Threading;
using Icon.Maui.Core.Validations;
using Icon.Maui.Models.User;
using Icon.Maui.Services.Permission;
using Icon.Maui.Services.User;

namespace Icon.Maui.Pages.User;

public partial class CreateOrEditUserModal : ModalBase
{
    public override string ModalId => "create-or-edit-user";

    [Parameter] public EventCallback OnSave { get; set; }

    protected IPermissionService PermissionService;
    protected IUserAppService UserAppService;
    protected IUserProfileService UserProfileService;

    public CreateOrEditUserModel CreateOrEditUserModel { get; set; } = new();
        
    private bool _isDeleteButtonVisible;
    private bool _isUnlockButtonVisible;
    public CreateOrEditUserModal()
    {
        PermissionService = Resolve<IPermissionService>();
        UserAppService = Resolve<IUserAppService>();
        UserProfileService = Resolve<IUserProfileService>();
        
    }

    public async Task OpenFor(UserListModel user)
    {
        await SetBusyAsync(async () =>
        {
            await WebRequestExecuter.Execute(
                async () => await UserAppService.GetUserForEdit(new NullableIdDto<long>(user?.Id)),
                async (getUserForEditOutput) =>
                {
                    InitializeModel(getUserForEditOutput);
                    
                    if (CreateOrEditUserModel.IsNewUser)
                    {
                        CreateOrEditUserModel.Photo = UserProfileService.GetDefaultProfilePicture();
                        CreateOrEditUserModel.IsActive = true;
                        CreateOrEditUserModel.IsLockoutEnabled = true;
                        CreateOrEditUserModel.ShouldChangePasswordOnNextLogin = true;
                    }
                    else
                    {
                        CreateOrEditUserModel.Photo = user.Photo;
                        CreateOrEditUserModel.CreationTime = user.CreationTime;
                        CreateOrEditUserModel.IsEmailConfirmed = user.IsEmailConfirmed;
                    }
                    
                    await Show();
                }
            );
        });
    }

    private void InitializeModel(GetUserForEditOutput getUserForEditOutput)
    {
        CreateOrEditUserModel = ObjectMapper.Map<CreateOrEditUserModel>(getUserForEditOutput.User);
        CreateOrEditUserModel.Roles = getUserForEditOutput.Roles;
        CreateOrEditUserModel.MemberedOrganizationUnits = getUserForEditOutput.MemberedOrganizationUnits;
        
        CreateOrEditUserModel.IsNewUser = CreateOrEditUserModel.Id == null;
        
        CreateOrEditUserModel.SelectedOrganizationUnits = ObjectMapper.Map<List<OrganizationUnitModel>>(getUserForEditOutput.AllOrganizationUnits);
    }
    
    private async Task SaveUser()
    {
        var user = ObjectMapper.Map<UserEditDto>(CreateOrEditUserModel);

        var input = new CreateOrUpdateUserInput
        {
            User = user,
            AssignedRoleNames = CreateOrEditUserModel.Roles.Where(x => x.IsAssigned).Select(x => x.RoleName).ToArray(),
            OrganizationUnits = CreateOrEditUserModel.SelectedOrganizationUnits.Where(x => x.IsAssigned).Select(x => x.Id).ToList(),
            SendActivationEmail = CreateOrEditUserModel.SendActivationEmail,
            SetRandomPassword = CreateOrEditUserModel.SetRandomPassword
        };
        
        await SetBusyAsync(async () =>
        {
            await WebRequestExecuter.Execute(
                async () => await UserAppService.CreateOrUpdateUser(input),
                async () =>
                {
                    await UserDialogsService.AlertSuccess(L("SuccessfullySaved"));
                    await Hide();
                    await OnSave.InvokeAsync();
                }
            );
        });
    }

    private string OUCodeIntentConverter(string code)
    {
        var resultWithIndent = "";

        var indentCharacter = ".";
        foreach (var character in code)
        {
            if (character == '.')
            {
                resultWithIndent += indentCharacter;
            }
        }

        return resultWithIndent;
    }

    private async Task UnlockUser()
    {
        if (!CreateOrEditUserModel.Id.HasValue)
        {
            return;
        }

        await SetBusyAsync(async () =>
        {
            await WebRequestExecuter.Execute(
                async () => await UserAppService.UnlockUser(new EntityDto<long>(CreateOrEditUserModel.Id.Value)),
                async () =>
                {
                    await UserDialogsService.AlertSuccess(L("UnlockedTheUser", CreateOrEditUserModel.UserName));
                    await Hide();
                }
            );
        });
    }

    public async Task DeleteUser()
    {
        if (!CreateOrEditUserModel.Id.HasValue)
        {
            return;
        }

        var isConfirmed = await UserDialogsService.Confirm(L("UserDeleteWarningMessage", "\"" + CreateOrEditUserModel.UserName + "\""), L("AreYouSure"));
        if (isConfirmed)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestExecuter.Execute(
                    async () => await UserAppService.DeleteUser(new EntityDto<long>(CreateOrEditUserModel.Id.Value)),
                    async () =>
                    {
                        await UserDialogsService.AlertSuccess(L("SuccessfullyDeleted"));
                        await Hide();
                        await OnSave.InvokeAsync();
                    }
                );
            });
        }
    }
}