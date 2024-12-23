using System;
using System.Linq;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityProperties;
using Abp.EntityHistory;
using Abp.Extensions;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using Icon.Auditing.Dto;
using Icon.Authorization.Accounts.Dto;
using Icon.Authorization.Delegation;
using Icon.Authorization.Permissions.Dto;
using Icon.Authorization.Roles;
using Icon.Authorization.Roles.Dto;
using Icon.Authorization.Users;
using Icon.Authorization.Users.Delegation.Dto;
using Icon.Authorization.Users.Dto;
using Icon.Authorization.Users.Importing.Dto;
using Icon.Authorization.Users.Profile.Dto;
using Icon.Chat;
using Icon.Chat.Dto;
using Icon.Common.Dto;
using Icon.DynamicEntityProperties.Dto;
using Icon.Editions;
using Icon.Editions.Dto;
using Icon.EntityChanges;
using Icon.EntityChanges.Dto;
using Icon.Friendships;
using Icon.Friendships.Cache;
using Icon.Friendships.Dto;
using Icon.Localization.Dto;
using Icon.Matrix;
using Icon.Matrix.Models;

using Icon.Matrix.Portal.Dto;
using Icon.MultiTenancy;
using Icon.MultiTenancy.Dto;
using Icon.MultiTenancy.HostDashboard.Dto;
using Icon.MultiTenancy.Payments;
using Icon.MultiTenancy.Payments.Dto;
using Icon.Notifications.Dto;
using Icon.Organizations.Dto;
using Icon.Sessions.Dto;
using Icon.WebHooks.Dto;

namespace Icon
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {

            configuration.CreateMap<Memory, MemoryDto>().ReverseMap();
            configuration.CreateMap<Memory, MemoryListDto>().ReverseMap();
            configuration.CreateMap<MemoryType, MemoryTypeDto>().ReverseMap();
            configuration.CreateMap<MemoryProcess, MemoryProcessDto>().ReverseMap();
            configuration.CreateMap<MemoryProcessStep, MemoryProcessStepDto>().ReverseMap();
            configuration.CreateMap<MemoryProcessLog, MemoryProcessLogDto>().ReverseMap();

            configuration.CreateMap<Platform, PlatformDto>().ReverseMap();

            configuration.CreateMap<Character, CharacterDto>().ReverseMap();
            configuration.CreateMap<Character, CharacterListDto>().ReverseMap();
            configuration.CreateMap<Character, CharacterSimpleDto>().ReverseMap();
            configuration.CreateMap<CharacterBio, CharacterBioDto>().ReverseMap();

            configuration.CreateMap<CharacterPersona, CharacterPersonaDto>().ReverseMap();
            configuration.CreateMap<CharacterPersona, CharacterPersonaListDto>().ReverseMap();
            configuration.CreateMap<CharacterPersonaTwitterRank, CharacterPersonaTwitterRankDto>().ReverseMap();

            configuration.CreateMap<Persona, PersonaSimpleDto>().ReverseMap();
            configuration.CreateMap<Persona, PersonaDto>()
                .ForMember(dto => dto.PlatformNames, options => options.MapFrom(e => string.Join(", ", e.Platforms.Where(p => !String.IsNullOrEmpty(p.PlatformPersonaId)).Select(p => p.Platform.Name))));
            configuration.CreateMap<PersonaPlatform, PersonaPlatformDto>().ReverseMap();

            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();


            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>()
                .ReverseMap()
                .ForMember(dto => dto.TotalAmount, options => options.MapFrom(e => e.GetTotalAmount()));
            configuration.CreateMap<SubscriptionPaymentProductDto, SubscriptionPaymentProduct>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>().ForMember(x => x.Roles, options => options.Ignore());
            configuration.CreateMap<User, FindUsersOutputDto>();
            configuration.CreateMap<User, FindOrganizationUnitUsersOutputDto>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();

            //EntityChanges
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityChange, EntityAndPropertyChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();
            configuration.CreateMap<EntityChangePropertyAndUser, EntityChangeListDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicProperty, DynamicPropertyDto>().ReverseMap();
            configuration.CreateMap<DynamicPropertyValue, DynamicPropertyValueDto>().ReverseMap();
            configuration.CreateMap<DynamicEntityProperty, DynamicEntityPropertyDto>()
                .ForMember(dto => dto.DynamicPropertyName,
                    options => options.MapFrom(entity =>
                        entity.DynamicProperty.DisplayName.IsNullOrEmpty()
                            ? entity.DynamicProperty.PropertyName
                            : entity.DynamicProperty.DisplayName));
            configuration.CreateMap<DynamicEntityPropertyDto, DynamicEntityProperty>();

            configuration.CreateMap<DynamicEntityPropertyValue, DynamicEntityPropertyValueDto>().ReverseMap();

            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}