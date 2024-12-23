// using System;
// using System.Collections.Generic;



// namespace Icon.BaseManagement.Builders.Forms
// {
//     public static partial class ChangeRequestForm
//     {

//         public static BaseFormSectionDto GetSetupSection() => new BaseFormSectionDto
//         {
//             SectionTitle = "Setup",
//             IsHidden = true,
//             Rows = new List<BaseFormRowDto>
//             {
//                 new BaseFormRowDto
//                 {
//                     Fields = new List<BaseFormFieldDto>
//                     {
//                         GetChangeRequestId(),
//                         GetChangeRequestTypeId(),
//                         GetChangeRequestEntityId(),
//                         GetChangeRequestEntityType(),
//                         GetChangeRequestTranslationClass(),
//                         GetChangeRequestTranslationClassVersion(),

//                         GetLocationId(),
//                         GetClientId(),

//                         GetFromLocationId(),
//                         GetToLocationId(),
//                     }
//                 }
//             }
//         };

//         public static BaseFormSectionDto GetChangeRequestInfoSection() => new BaseFormSectionDto
//         {
//             SectionTitle = "ChangeRequestInformation",
//             Rows = new List<BaseFormRowDto>
//             {
//                 new BaseFormRowDto
//                 {
//                     Fields = new List<BaseFormFieldDto>
//                     {
//                         GetChangeRequestChangeTypeName(),
//                         GetChangeRequestChangeStatusName(),
//                         GetChangeRequestChangeHandleBeforeDate(),
//                         GetChangeRequestChangeImplementOnDate(),


//                         //GetChangeRequestChangeHistories(),
//                     }
//                 }
//             }
//         };

//         public static BaseFormSectionDto GetLocationInfoSection() => new BaseFormSectionDto
//         {
//             SectionTitle = "LocationInformation",
//             Rows = new List<BaseFormRowDto>
//             {
//                 new BaseFormRowDto
//                 {
//                     Fields = new List<BaseFormFieldDto>
//                     {
//                         GetLocationName(),
//                         GetLocationAddress(),
//                     }
//                 }
//             }
//         };

//         public static BaseFormSectionDto GetClientInfoSection() => new BaseFormSectionDto
//         {
//             SectionTitle = "ClientInformation",
//             Rows = new List<BaseFormRowDto>
//             {
//                 new BaseFormRowDto
//                 {
//                     Fields = new List<BaseFormFieldDto>
//                     {
//                         GetClientExternalId(),
//                         GetClientFirstName(),
//                         GetClientLastName(),
//                         GetClientDateOfBirth(),
//                     }
//                 }
//             }
//         };

//         public static BaseFormSectionDto GetChangeRequestRemarksSection() => new BaseFormSectionDto
//         {
//             SectionTitle = "ChangeRequestRemarks",
//             Rows = new List<BaseFormRowDto>
//             {
//                 new BaseFormRowDto
//                 {
//                     Fields = new List<BaseFormFieldDto>
//                     {
//                         GetChangeRequestCreatorComments(),
//                         GetChangeRequestHandlerComments(),
//                     }
//                 }
//             }
//         };

//         public static BaseFormFieldDto GetChangeRequestId() => BaseFormFieldFactory.CreateTextField(
//             fieldName: nameof(ChangeRequestDto.Id),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, Guid>(f => f.Id),
//             columnWidth: 3,
//             isDisabled: true,
//             isHidden: true
//         );

//         public static BaseFormFieldDto GetChangeRequestTypeId() => BaseFormFieldFactory.CreateTextField(
//             fieldName: nameof(ChangeRequestDto.ChangeType) + nameof(ChangeRequestTypeDto.Id),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, Guid>(f => f.ChangeType.Id),
//             columnWidth: 3,
//             isDisabled: true,
//             isHidden: true
//         );

//         public static BaseFormFieldDto GetLocationId() => BaseFormFieldFactory.CreateTextField(
//             fieldName: nameof(ChangeRequestDto.Location) + nameof(ChangeRequestLocationDto.Id),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, Guid>(f => f.Location.Id),
//             columnWidth: 3,
//             isDisabled: true,
//             isHidden: true
//         );

//         public static BaseFormFieldDto GetClientId() => BaseFormFieldFactory.CreateTextField(
//             fieldName: nameof(ChangeRequestDto.Client) + nameof(ChangeRequestClientDto.Id),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, Guid>(f => f.Client.Id),
//             columnWidth: 3,
//             isDisabled: true,
//             isHidden: true
//         );

//         public static BaseFormFieldDto GetChangeRequestEntityId() => BaseFormFieldFactory.CreateTextField(
//             fieldName: nameof(ChangeRequestDto.EntityId),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, Guid>(f => f.EntityId),
//             columnWidth: 3,
//             isDisabled: true,
//             isHidden: true
//         );

//         public static BaseFormFieldDto GetChangeRequestEntityType() => BaseFormFieldFactory.CreateNumberField(
//             fieldName: nameof(ChangeRequestDto.EntityType),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, ChangeRequestEntityEnum>(f => f.EntityType),
//             columnWidth: 3,
//             isDisabled: true,
//             isHidden: true
//         );

//         public static BaseFormFieldDto GetChangeRequestChangeHandleBeforeDate() => BaseFormFieldFactory.CreateDateField(
//             fieldName: nameof(ChangeRequestDto.ChangeHandleBeforeDate),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, DateTime>(f => f.ChangeHandleBeforeDate),
//             columnWidth: 3,
//             isDisabled: true,
//             isHidden: false
//         );

//         public static BaseFormFieldDto GetChangeRequestChangeImplementOnDate() => BaseFormFieldFactory.CreateDateField(
//             fieldName: nameof(ChangeRequestDto.ChangeImplementOnDate),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, DateTime>(f => f.ChangeImplementOnDate),
//             columnWidth: 3,
//             isDisabled: true,
//             isHidden: false
//         );

//         public static BaseFormFieldDto GetChangeRequestTranslationClass() => BaseFormFieldFactory.CreateTextField(
//             fieldName: nameof(ChangeRequestDto.TranslationClass),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, string>(f => f.TranslationClass),
//             columnWidth: 3,
//             isDisabled: true,
//             isHidden: true
//         );

//         public static BaseFormFieldDto GetChangeRequestTranslationClassVersion() => BaseFormFieldFactory.CreateTextField(
//             fieldName: nameof(ChangeRequestDto.TranslationClassVersion),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, int>(f => f.TranslationClassVersion),
//             columnWidth: 3,
//             isDisabled: true,
//             isHidden: true
//         );

//         public static BaseFormFieldDto GetChangeRequestCreatorComments() => BaseFormFieldFactory.CreateTextAreaField(
//             fieldName: nameof(ChangeRequestDto.CreatorComments),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, string>(f => f.CreatorComments),
//             columnWidth: 6
//         );

//         public static BaseFormFieldDto GetChangeRequestHandlerComments() => BaseFormFieldFactory.CreateTextAreaField(
//             fieldName: nameof(ChangeRequestDto.HandlerComments),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, string>(f => f.HandlerComments),
//             columnWidth: 6
//         );

//         public static BaseFormFieldDto GetLocationName() => BaseFormFieldFactory.CreateTextField(
//             fieldName: nameof(ChangeRequestDto.Location) + nameof(ChangeRequestLocationDto.Name),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, string>(f => f.Location.Name),
//             columnWidth: 6,
//             isDisabled: true
//         );

//         public static BaseFormFieldDto GetLocationAddress() => BaseFormFieldFactory.CreateTextField(
//             fieldName: nameof(ChangeRequestDto.Location) + nameof(ChangeRequestLocationDto.Address),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, string>(f => f.Location.Address),
//             columnWidth: 6,
//             isDisabled: true
//         );


//         public static BaseFormFieldDto GetChangeRequestChangeTypeName() => BaseFormFieldFactory.CreateTextField(
//             fieldName: nameof(ChangeRequestDto.ChangeType),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, string>(f => f.ChangeType.Name),
//             columnWidth: 3,
//             isDisabled: true,
//             shouldTranslate: true
//         );

//         public static BaseFormFieldDto GetChangeRequestChangeStatusName() => BaseFormFieldFactory.CreateTextField(
//             fieldName: nameof(ChangeRequestDto.ChangeStatus),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, string>(f => f.ChangeStatus.Name),
//             columnWidth: 3,
//             isDisabled: true,
//             shouldTranslate: true
//         );

//         public static BaseFormFieldDto GetChangeRequestChangeHistories() => BaseFormFieldFactory.CreateTextField(
//             fieldName: nameof(ChangeRequestDto.ChangeHistories),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, IList<ChangeRequestHistoryDto>>(f => f.ChangeHistories),
//             columnWidth: 6,
//             isDisabled: true
//         );

//         public static BaseFormFieldDto GetClientExternalId() => BaseFormFieldFactory.CreateNumberField(
//             fieldName: nameof(ChangeRequestDto.Client) + nameof(ChangeRequestClientDto.ExternalId),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, int>(f => f.Client.ExternalId),
//             columnWidth: 3,
//             isDisabled: true
//         );

//         public static BaseFormFieldDto GetClientFirstName() => BaseFormFieldFactory.CreateTextField(
//             fieldName: nameof(ChangeRequestDto.Client) + nameof(ChangeRequestClientDto.FirstName),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, string>(f => f.Client.FirstName),
//             columnWidth: 3,
//             isDisabled: true
//         );

//         public static BaseFormFieldDto GetClientLastName() => BaseFormFieldFactory.CreateTextField(
//             fieldName: nameof(ChangeRequestDto.Client) + nameof(ChangeRequestClientDto.LastName),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, string>(f => f.Client.LastName),
//             columnWidth: 3,
//             isDisabled: true
//         );

//         public static BaseFormFieldDto GetClientDateOfBirth() => BaseFormFieldFactory.CreateDateField(
//             fieldName: nameof(ChangeRequestDto.Client) + nameof(ChangeRequestClientDto.DateOfBirth),
//             valuePath: BaseHelper.GetPropertyPath<ChangeRequestDto, DateTime?>(f => f.Client.DateOfBirth),
//             columnWidth: 3,
//             isDisabled: true
//         );

//         // public static BaseFormFieldDto GetTripExternalId() => BaseFormFieldFactory.CreateNumberField(
//         //     fieldName: "Trip" + nameof(ChangeRequestLocationTripDto.ExternalId),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, int>(f => f.ExternalId),
//         //     columnWidth: 3,
//         //     isDisabled: true
//         // );

//         // public static BaseFormFieldDto GetTripStatusName() => BaseFormFieldFactory.CreateTextField(
//         //     fieldName: nameof(ChangeRequestLocationTripDto.TripStatusName),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, string>(f => f.TripStatusName),
//         //     columnWidth: 3,
//         //     isDisabled: true,
//         //     shouldTranslate: true
//         // );

//         // public static BaseFormFieldDto GetFromLocationId() => BaseFormFieldFactory.CreateTextField(
//         //     fieldName: nameof(ChangeRequestLocationTripDto.UpdatedData.FromLocationId),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, Guid?>(f => f.UpdatedData.FromLocationId),
//         //     isHidden: true
//         // );

//         // public static BaseFormFieldDto GetTripFromStreet(bool isDisabled = false) => BaseFormFieldFactory.CreateTextField(
//         //     fieldName: nameof(ChangeRequestLocationTripDto.UpdatedData.FromStreet),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, string>(f => f.UpdatedData.FromStreet),
//         //     columnWidth: 6,
//         //     isDisabled: isDisabled
//         // );

//         // public static BaseFormFieldDto GetTripFromStreetNumber(bool isDisabled = false) => BaseFormFieldFactory.CreateTextField(
//         //     fieldName: nameof(ChangeRequestLocationTripDto.UpdatedData.FromStreetNumber),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, string>(f => f.UpdatedData.FromStreetNumber),
//         //     columnWidth: 3,
//         //     isDisabled: isDisabled

//         // );

//         // public static BaseFormFieldDto GetTripFromStreetNumberExtra(bool isDisabled = false) => BaseFormFieldFactory.CreateTextField(
//         //     fieldName: nameof(ChangeRequestLocationTripDto.UpdatedData.FromStreetNumberExtra),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, string>(f => f.UpdatedData.FromStreetNumberExtra),
//         //     columnWidth: 3,
//         //     isDisabled: isDisabled
//         // );

//         // public static BaseFormFieldDto GetTripFromZipCode(bool isDisabled = false) => BaseFormFieldFactory.CreateTextField(
//         //     fieldName: nameof(ChangeRequestLocationTripDto.UpdatedData.FromZipCode),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, string>(f => f.UpdatedData.FromZipCode),
//         //     columnWidth: 2,
//         //     isDisabled: isDisabled
//         // );

//         // public static BaseFormFieldDto GetTripFromCity(bool isDisabled = false) => BaseFormFieldFactory.CreateTextField(
//         //     fieldName: nameof(ChangeRequestLocationTripDto.UpdatedData.FromCity),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, string>(f => f.UpdatedData.FromCity),
//         //     columnWidth: 4,
//         //     isDisabled: isDisabled
//         // );

//         // public static BaseFormFieldDto GetTripScheduledStartDate(bool isDisabled = false) => BaseFormFieldFactory.CreateDateField(
//         //     fieldName: nameof(ChangeRequestLocationTripDto.UpdatedData.ScheduledStartTime),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, DateTime?>(f => f.UpdatedData.ScheduledStartTime),
//         //     columnWidth: 4,
//         //     isDisabled: isDisabled
//         // );

//         // public static BaseFormFieldDto GetTripScheduledStartTime(bool isDisabled = false) => BaseFormFieldFactory.CreateTimeField(
//         //     fieldName: nameof(ChangeRequestLocationTripDto.UpdatedData.ScheduledStartTime),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, DateTime?>(f => f.UpdatedData.ScheduledStartTime),
//         //     columnWidth: 2,
//         //     isDisabled: isDisabled
//         // );

//         // public static BaseFormFieldDto GetToLocationId() => BaseFormFieldFactory.CreateTextField(
//         //     fieldName: nameof(ChangeRequestLocationTripDto.UpdatedData.ToLocationId),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, Guid?>(f => f.UpdatedData.ToLocationId),
//         //     isHidden: true
//         // );

//         // public static BaseFormFieldDto GetTripToStreet(bool isDisabled = false) => BaseFormFieldFactory.CreateTextField(
//         //     fieldName: nameof(ChangeRequestLocationTripDto.UpdatedData.ToStreet),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, string>(f => f.UpdatedData.ToStreet),
//         //     columnWidth: 6,
//         //     isDisabled: isDisabled
//         // );

//         // public static BaseFormFieldDto GetTripToStreetNumber(bool isDisabled = false) => BaseFormFieldFactory.CreateTextField(
//         //     fieldName: nameof(ChangeRequestLocationTripDto.UpdatedData.ToStreetNumber),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, string>(f => f.UpdatedData.ToStreetNumber),
//         //     columnWidth: 3,
//         //     isDisabled: isDisabled
//         // );

//         // public static BaseFormFieldDto GetTripToStreetNumberExtra(bool isDisabled = false) => BaseFormFieldFactory.CreateTextField(
//         //     fieldName: nameof(ChangeRequestLocationTripDto.UpdatedData.ToStreetNumberExtra),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, string>(f => f.UpdatedData.ToStreetNumberExtra),
//         //     columnWidth: 3,
//         //     isDisabled: isDisabled
//         // );

//         // public static BaseFormFieldDto GetTripToZipCode(bool isDisabled = false) => BaseFormFieldFactory.CreateTextField(
//         //     fieldName: nameof(ChangeRequestLocationTripDto.UpdatedData.ToZipCode),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, string>(f => f.UpdatedData.ToZipCode),
//         //     columnWidth: 2,
//         //     isDisabled: isDisabled
//         // );

//         // public static BaseFormFieldDto GetTripToCity(bool isDisabled = false) => BaseFormFieldFactory.CreateTextField(
//         //     fieldName: nameof(ChangeRequestLocationTripDto.UpdatedData.ToCity),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, string>(f => f.UpdatedData.ToCity),
//         //     columnWidth: 4,
//         //     isDisabled: isDisabled
//         // );

//         // public static BaseFormFieldDto GetTripScheduledEndDate(bool isDisabled = false) => BaseFormFieldFactory.CreateDateField(
//         //     fieldName: nameof(ChangeRequestLocationTripDto.UpdatedData.ScheduledEndTime),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, DateTime?>(f => f.UpdatedData.ScheduledEndTime),
//         //     columnWidth: 4,
//         //     isDisabled: isDisabled
//         // );

//         // public static BaseFormFieldDto GetTripScheduledEndTime(bool isDisabled = false) => BaseFormFieldFactory.CreateTimeField(
//         //     fieldName: nameof(ChangeRequestLocationTripDto.UpdatedData.ScheduledEndTime),
//         //     valuePath: BaseHelper.GetPropertyPath<ChangeRequestLocationTripDto, DateTime?>(f => f.UpdatedData.ScheduledEndTime),
//         //     columnWidth: 2,
//         //     isDisabled: isDisabled
//         // );

//     }
// }
